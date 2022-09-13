$(function () {
    $("#btnshowpass").tooltip();

    $("#btnshowpass").click(function(){
        if ($("#iconshow").hasClass('bi-eye-fill'))
        {
            $('#pass').attr('type', 'text');
            $('#iconshow').addClass('bi-eye-slash-fill').removeClass('bi-eye-fill');
        }
        else
        {
            $('#pass').attr('type', 'password');
            $('#iconshow').addClass('bi-eye-fill').removeClass('bi-eye-slash-fill');
        }
    });
});