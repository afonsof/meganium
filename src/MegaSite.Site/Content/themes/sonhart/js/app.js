$(document).ready(function () {
    var $container = $('#portfolioList');
    $container.on('layoutComplete', function () {
        $container.masonry({
            itemSelector: '.item'
        });
    });
    var over = false;

    $container.on('click', 'input', function () {
        var len = $container.find('input:checked').length;
        if (len > limit) {
            if (!over) {
                if (confirm("Você já selecionou a quantidade limite de fotos, deseja continuar escolhendo? " +
                    "Isto pode gerar custos extra para o seu álbum.")) {
                    over = true;
                    $('#photoSelection').css('color', 'red');
                    $('#photoSelection').find('small').show();
                } else {
                    $(this).removeAttr('checked');
                }
            }
        } else {
            over = false;
            $('#photoSelection').css('color', '#d5b03f');
            $('#photoSelection').find('small').hide();
        }
        len = $container.find('input:checked').length;
        $('#photoSelection').find('span').html(len + " de " + limit + " fotos");
    });

    $('#submit-button').on('click', function() {
        var inputs = $container.find('input:checked');

        var titles = '';
        for (var i = 0; i < inputs.length; i++) {
            var input = $(inputs[i]);
            var title = input.closest('li').find('a').attr('title');
            titles += title + '\n';
        }
        $('#titles').val(titles);
        $(this).closest('form').submit();
    });
});