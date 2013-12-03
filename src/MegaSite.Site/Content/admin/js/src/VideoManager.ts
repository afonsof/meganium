/// <reference path="../definitions/jquery/jquery.d.ts" />

class VideoManager {
    constructor(private element: JQuery, private video: JQuery, private button: JQuery) {
        var timer: number;
        var lastVal: string;

        element.on('keyup', function () {
            var value: string = $(this).val();

            if (!value) {
                video.html('');
                return;
            }
            if (value == lastVal) {
                return;
            }
            video.html('Carregando...');
            clearTimeout(timer);
            timer = setTimeout(function () {
                lastVal = value;
                var regex: RegExp = /^(https?:\/\/)?(www.)?((youtube.com(.[a-z]{2})?|youtu.be)\/(watch\?(?:[\w_\-=&]+)?v=|embed\/)?)?([\w_\-]{11})$/;
                var matches = regex.exec(value);
                var id = matches ? matches[7] : null;
                if (id) {
                    video.html('<iframe width="560" height="315" src="//www.youtube.com/embed/' + id + '" frameborder="0" allowfullscreen></iframe>');
                    button.removeAttr('disabled');
                } else {
                    video.html('Não é um vídeo válido.');
                    button.attr('disabled', 'disabled');
                }
            }, 500);
        });
    }
}