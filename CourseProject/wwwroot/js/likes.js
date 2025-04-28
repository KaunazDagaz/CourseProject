const likeService = {
    toggleLike: function (templateId) {
        $.ajax({
            url: '/Like/Like',
            type: 'POST',
            data: { templateId: templateId },
            success: function (response) {
                if (response.success) {
                    const $likeIcon = $("#like-button i");
                    const $likeCount = $("#like-count");
                    const currentCount = parseInt($likeCount.text());
                    const newCount = response.userLiked ? currentCount + 1 : currentCount - 1;
                    $likeCount.text(newCount);
                    if (response.userLiked) {
                        $likeIcon.removeClass("far").addClass("fas");
                    } else {
                        $likeIcon.removeClass("fas").addClass("far");
                    }
                } else {
                    showMessage(i18next.t("LikePostFailed"));
                }
            },
            error: function () {
                showMessage(i18next.t("LikePostFailed"));
            }
        });
    },
};

$(document).ready(function () {
    if ($("#like-button").length > 0) {
        $("#like-button").on("click", function (e) {
            e.preventDefault();
            const templateId = $(this).data("template-id");
            likeService.toggleLike(templateId);
        });
    }
});