document.addEventListener('DOMContentLoaded', function () {
    let formCount = 0;
    let sortable;
    i18next.on('languageChanged', function() {
        updatePositions();
    });    
    addNewForm();
    initSortable();

    document.getElementById('addFormBtn').addEventListener('click', function () {
        addNewForm();
    });

    document.getElementById('formsContainer').addEventListener('submit', function (e) {
        updatePositions();
        const formContainer = document.getElementById('formsList');
        const requiredFields = formContainer.querySelectorAll('[required]');
        let isValid = true;
        let invalidFields = [];
        document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                isValid = false;
                field.classList.add('is-invalid');
                invalidFields.push(field.name);
            } else {
                field.classList.remove('is-invalid');
            }
        });

        if (!isValid) {
            e.preventDefault();
            return;
        }
    });

    document.getElementById('formsList').addEventListener('input', function (e) {
        if (e.target && (e.target.tagName === 'INPUT' || e.target.tagName === 'SELECT' || e.target.tagName === 'TEXTAREA')) {
            if (e.target.value.trim()) {
                e.target.classList.remove('is-invalid');
            }
        }
    });

    document.getElementById('formsList').addEventListener('change', function (e) {
        if (e.target && e.target.classList.contains('question-type-select')) {
            handleQuestionTypeChange(e.target);
            if (e.target.value.trim()) {
                e.target.classList.remove('is-invalid');
            }
        }
    });

    document.getElementById('formsList').addEventListener('click', function (e) {
        if (e.target && e.target.closest('.add-checkbox-option')) {
            const button = e.target.closest('.add-checkbox-option');
            addCheckboxOption(button.closest('.checkbox-settings').querySelector('.checkbox-options-container'),
                button.closest('.form-container'));
        }
        if (e.target && e.target.closest('.remove-checkbox-option')) {
            const button = e.target.closest('.remove-checkbox-option');
            const container = button.closest('.checkbox-options-container');
            if (container.children.length > 1) {
                button.closest('.checkbox-option-row').remove();
                updateCheckboxOptionIndices(container);
            }
        }
    });

    function initSortable() {
        const formsList = document.getElementById('formsList');
        sortable = new Sortable(formsList, {
            animation: 150,
            handle: '.card-header',
            ghostClass: 'bg-light',
            onEnd: function () {
                updatePositions();
            }
        });
    }

    function updatePositions() {
        const formContainers = document.querySelectorAll('.form-container');
        formContainers.forEach(function (container, index) {
            const positionInput = container.querySelector('[name$=".Position"]');
            if (positionInput) {
                positionInput.value = index;
            }
            const formNumberSpan = container.querySelector('.form-number');
            const translatedText = i18next.t('FormNumber', { number: index + 1 });
            formNumberSpan.textContent = translatedText || `Form ${index + 1}`;
        });
    }

    function updateCheckboxOptionIndices(container) {
        const formIndex = container.closest('.form-container').querySelector('[name$=".Position"]').name.match(/\[(\d+)\]/)[1];
        const optionInputs = container.querySelectorAll('input[name^="[' + formIndex + '].Question.CheckboxOptions"]');
        optionInputs.forEach((input, index) => {
            input.name = `[${formIndex}].Question.CheckboxOptions[${index}]`;
        });
    }

    function handleQuestionTypeChange(selectElement) {
        const formContainer = selectElement.closest('.form-container');
        const selectedType = selectElement.value;
        const typeSettings = formContainer.querySelectorAll('.type-settings');
        typeSettings.forEach(setting => {
            setting.classList.add('d-none');
        });
        switch (selectedType) {
            case 'SingleLine':
                formContainer.querySelector('.singleline-settings').classList.remove('d-none');
                break;
            case 'Integer':
                formContainer.querySelector('.integer-settings').classList.remove('d-none');
                break;
            case 'Checkbox':
                const checkboxSettings = formContainer.querySelector('.checkbox-settings');
                checkboxSettings.classList.remove('d-none');
                const optionsContainer = checkboxSettings.querySelector('.checkbox-options-container');
                if (optionsContainer.children.length === 0) {
                    addCheckboxOption(optionsContainer, formContainer);
                }
                break;
        }
    }

    function addCheckboxOption(container, formContainer) {
        const formIndex = formContainer.querySelector('[name$=".Position"]').name.match(/\[(\d+)\]/)[1];
        const optionIndex = container.children.length;
        const optionRow = document.createElement('div');
        optionRow.className = 'checkbox-option-row input-group mb-2';
        optionRow.innerHTML = `
            <input type="text" name="[${formIndex}].Question.CheckboxOptions[${optionIndex}]" 
                class="form-control" required/>
            <button type="button" class="btn btn-outline-danger remove-checkbox-option">
               <i class="fas fa-trash-alt"></i>
            </button>
        `;
        container.appendChild(optionRow);
    }

    function addNewForm() {
        const formIndex = formCount;
        formCount++;
        const templateContent = document.getElementById('formTemplate').innerHTML;
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = templateContent;
        const newForm = tempDiv.firstElementChild;
        const formNumberSpan = newForm.querySelector('.form-number');
        const translatedText = i18next.t('FormNumber', { number: formCount });
        formNumberSpan.textContent = translatedText || `Form ${formCount}`;
        newForm.querySelectorAll('[name]').forEach(function (element) {
            const name = element.getAttribute('name').replace('IDX', formIndex);
            element.setAttribute('name', name);
            if (name.endsWith('.Position')) {
                element.value = formIndex;
            }
        });
        newForm.querySelector('.remove-form-btn').addEventListener('click', function () {
            if (formCount > 1) {
                this.closest('.form-container').remove();
                formCount--;
                updatePositions();
            }
        });
        document.getElementById('formsList').appendChild(newForm);
        const dragHandle = document.createElement('div');
        dragHandle.className = 'drag-handle me-2';
        dragHandle.style.cursor = 'grab';
        newForm.querySelector('.card-header').prepend(dragHandle);
        updatePositions();
    }
});