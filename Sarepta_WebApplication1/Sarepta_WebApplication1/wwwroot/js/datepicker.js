jQuery(document).ready(function ($) {
    $(function () {
        $("#datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: '1920:2020',
            dateFormat: 'yy-mm-dd'
        });
    });
});