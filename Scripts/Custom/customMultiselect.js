document.addEventListener('DOMContentLoaded', function () {
    // Dla każdego kontrolowanego elementu custom-multiselect
    document.querySelectorAll('.custom-multiselect').forEach(function (multiselect) {
        // Rozwijanie/zamykanie listy po kliknięciu w select-box
        var selectBox = multiselect.querySelector('.select-box');
        if (selectBox) {
            selectBox.addEventListener('click', function (e) {
                e.stopPropagation();
                multiselect.classList.toggle('active');
            });
        }

        // Obsługa wyszukiwania wewnątrz kontrolki
        var searchInput = multiselect.querySelector('.search-input');
        if (searchInput) {
            searchInput.addEventListener('input', function (e) {
                var searchTerm = e.target.value.toLowerCase();
                var options = multiselect.querySelectorAll('.option-item');
                options.forEach(function (option) {
                    var text = option.querySelector('span').textContent.toLowerCase();
                    option.style.display = text.includes(searchTerm) ? 'block' : 'none';
                });
            });
        }

        // Aktualizacja wyświetlanego tekstu wybranych opcji
        var checkboxes = multiselect.querySelectorAll('.option-item input');
        var selectedText = multiselect.querySelector('.selected-text');
        if (checkboxes && selectedText) {
            checkboxes.forEach(function (checkbox) {
                checkbox.addEventListener('change', function () {
                    var selected = Array.from(checkboxes)
                        .filter(function (c) { return c.checked; })
                        .map(function (c) { return c.nextElementSibling.textContent; });
                    // Ustalenie domyślnego tekstu w zależności od edytowanej listy
                    var defaultText = 'Select options...';
                    if (checkboxes.length > 0) {
                        if (checkboxes[0].name === "SelectedAuthorIds") {
                            defaultText = 'Select authors...';
                        } else if (checkboxes[0].name === "SelectedTagIds") {
                            defaultText = 'Select tags...';
                        }
                    }
                    selectedText.textContent = selected.length > 0 ? selected.join(', ') : defaultText;
                });
            });
        }
    });

    // Zamknięcie rozwiniętych kontrolek po kliknięciu poza nie
    document.addEventListener('click', function (e) {
        document.querySelectorAll('.custom-multiselect.active').forEach(function (multiselect) {
            if (!e.target.closest('.custom-multiselect')) {
                multiselect.classList.remove('active');
            }
        });
    });
});
