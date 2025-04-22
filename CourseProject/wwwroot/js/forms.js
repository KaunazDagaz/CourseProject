document.addEventListener('DOMContentLoaded', function () {
    let formCount = 0;
    let sortable;
    addNewForm();
    initSortable();

    document.getElementById('addFormBtn').addEventListener('click', function () {
        addNewForm();
    });

    document.getElementById('formsContainer').addEventListener('submit', function (e) {
        updatePositions();
        const formContainer = document.getElementById('formsList');
        const requiredFields = formContainer.querySelectorAll('input[required], select[required]');
        let isValid = true;
        let invalidFields = [];
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
            console.error('Invalid fields:', invalidFields);
            showMessage('Please fill in all required fields');
            return;
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
            container.querySelector('.form-number').textContent = `Form ${index + 1}`;
        });
    }

    function addNewForm() {
        const formIndex = formCount;
        formCount++;
        const templateContent = document.getElementById('formTemplate').innerHTML;
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = templateContent;
        const newForm = tempDiv.firstElementChild;
        newForm.querySelector('.form-number').textContent = `Form ${formCount}`;
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
            } else {
                showMessage('You must have at least one form');
            }
        });

        document.getElementById('formsList').appendChild(newForm);
        const dragHandle = document.createElement('div');
        dragHandle.className = 'drag-handle me-2';
        dragHandle.innerHTML = '<i class="bi bi-grip-vertical"></i>';
        dragHandle.style.cursor = 'grab';
        newForm.querySelector('.card-header').prepend(dragHandle);
        updatePositions();
    }
});