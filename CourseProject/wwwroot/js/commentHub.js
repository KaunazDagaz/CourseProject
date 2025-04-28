class CommentHubManager {
    constructor() {
        this.connection = null;
        this.templateId = null;
        this.initialized = false;
    }

    initialize(templateId) {
        this.templateId = templateId;
        if (!this.connection) {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/commentsHub")
                .withAutomaticReconnect()
                .build();
            this.connection.on("ReceiveComment", this.handleReceiveComment.bind(this));
            this.connection.onclose(() => {
                this.initialized = false;
            });
        }
        if (!this.initialized) {
            this.connection.start()
                .then(() => {
                    return this.connection.invoke("JoinTemplateGroup", this.templateId);
                })
                .then(() => {
                    this.initialized = true;
                });
            $(window).on("beforeunload", () => {
                if (this.connection && this.connection.state === "Connected") {
                    this.connection.invoke("LeaveTemplateGroup", this.templateId);
                }
            });
        } else if (this.connection.state === "Connected") {
            this.connection.invoke("JoinTemplateGroup", this.templateId);
        }
        return this;
    }

    joinTemplateGroup() {
        if (this.connection && this.connection.state === "Connected" && this.templateId) {
            return this.connection.invoke("JoinTemplateGroup", this.templateId);
        }
        return Promise.resolve();
    }

    leaveTemplateGroup() {
        if (this.connection && this.connection.state === "Connected" && this.templateId) {
            return this.connection.invoke("LeaveTemplateGroup", this.templateId);
        }
        return Promise.resolve();
    }

    handleReceiveComment(commentId, authorName, content, timestamp) {
        const commentHtml = `
            <div class="card mb-3" id="comment-${commentId}">
                <div class="card-header bg-light d-flex justify-content-between align-items-center py-2">
                    <div class="comment-author small">${authorName}</div>
                    <div class="comment-timestamp text-muted small" 
                         data-utc-time="${timestamp}">${timestamp}</div>
                </div>
                <div class="card-body py-2">
                    <p class="card-text comment-content mb-0">${content}</p>
                </div>
            </div>
        `;
        if ($(".comments-list .alert").length) {
            $(".comments-list .alert").remove();
        }
        $(".comments-list").append(commentHtml);
        convertTimestampsToLocalTime();
    }

    postComment(content, addCommentUrl) {
        if (!content || !this.templateId) return;
        $.ajax({
            url: addCommentUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                templateId: this.templateId,
                content: content
            }),
            success: function () {
                $("#new-comment-input").val('');
                showMessage(i18next.t("CommentPostSuccess"), false);
            },
            error: function () {
                showMessage(i18next.t("CommentPostFailed"), true);
            }
        });
    }
}

const commentHub = new CommentHubManager();