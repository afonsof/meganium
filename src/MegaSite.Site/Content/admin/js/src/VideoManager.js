/// <reference path="../definitions/jquery/jquery.d.ts" />
var VideoManager = (function () {
    function VideoManager(element, video, button, featuredMediaFile, title, content) {
        this.element = element;
        this.video = video;
        this.button = button;
        this.featuredMediaFile = featuredMediaFile;
        this.title = title;
        this.content = content;
        var _this = this;
        var timer;
        var lastVal;

        if (featuredMediaFile.val()) {
            this.renderVideo(JSON.parse(featuredMediaFile.val()));
        }

        element.on('keyup', function () {
            var value = $(this).val();

            if (!value) {
                video.html('');
                return;
            }
            if (value == lastVal) {
                return;
            }
            video.html('Carregando...');
            button.attr('disabled', 'disabled');
            featuredMediaFile.val('');

            clearTimeout(timer);
            timer = setTimeout(function () {
                lastVal = value;
                var id = _this.getIdFromUrl(value);
                if (id) {
                    _this.loadVideo(id);
                } else {
                    video.html('Não é um vídeo válido.');
                }
            }, 500);
        });
    }
    VideoManager.prototype.loadVideo = function (videoData) {
        var _this = this;

        if (videoData.ExternalServiceName == 'YoutubeVideos') {
            $.ajax({
                url: 'https://gdata.youtube.com/feeds/api/videos/' + videoData.ExternalServiceId + '?v=2&alt=json',
                type: 'GET',
                data: null,
                success: function (data) {
                    var entry = data.entry;
                    videoData.Title = entry.title.$t;
                    videoData.Description = entry.media$group.media$description.$t;

                    var maxWidth = 0;
                    var maxIndex = null;

                    for (var i = 0; i < entry.media$group.media$thumbnail.length; i++) {
                        var thumb = entry.media$group.media$thumbnail[i];
                        if (thumb.width > maxWidth) {
                            maxWidth = thumb.width;
                            maxIndex = i;
                        }
                    }
                    videoData.Url = entry.media$group.media$thumbnail[maxIndex].url;
                    _this.setFields(videoData);
                    _this.renderVideo(videoData);
                }
            });
        } else if (videoData.ExternalServiceName == 'VimeoVideos') {
            $.ajax({
                url: 'http://vimeo.com/api/v2/video/' + videoData.ExternalServiceId + '.json',
                type: 'GET',
                data: null,
                success: function (data) {
                    videoData.Title = data[0].title;
                    videoData.Description = data[0].description;
                    videoData.Url = data[0].thumbnail_large;
                    _this.setFields(videoData);
                    _this.renderVideo(videoData);
                }
            });
        }
    };

    VideoManager.prototype.setFields = function (videoData) {
        this.featuredMediaFile.val(JSON.stringify(videoData));
        this.title.val(videoData.Title);
        this.content.val(videoData.Description);
    };

    VideoManager.prototype.renderVideo = function (videoData) {
        var html = '';
        if (videoData.ExternalServiceName == 'YoutubeVideos') {
            html = '<iframe width="260" height="160" src="//www.youtube.com/embed/' + videoData.ExternalServiceId + '" frameborder="0" allowfullscreen></iframe>';
        } else if (videoData.ExternalServiceName == 'VimeoVideos') {
            html = '<iframe src = "//player.vimeo.com/video/' + videoData.ExternalServiceId + '?byline=0&amp;portrait=0&amp;badge=0&amp;color=ffffff" width="260" height="160" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>';
        }
        this.video.html(html);
        this.button.removeAttr('disabled');
    };

    VideoManager.prototype.getIdFromUrl = function (url) {
        var regex = /^(https?:\/\/)?(www.)?((youtube.com(.[a-z]{2})?|youtu.be)\/(watch\?(?:[\w_\-=&]+)?v=|embed\/)?)?([\w_\-]{11})$/;
        var matches = regex.exec(url);
        var id = matches ? matches[7] : null;

        if (id) {
            return {
                ExternalServiceId: id,
                ExternalServiceName: 'YoutubeVideos',
                Description: null,
                Title: null,
                Url: null
            };
        }

        regex = /vimeo\.com\/(\d+)/;
        matches = regex.exec(url);
        id = matches ? matches[1] : null;

        if (id) {
            return {
                ExternalServiceId: id,
                ExternalServiceName: 'VimeoVideos',
                Description: null,
                Title: null,
                Url: null
            };
        }
        return null;
    };
    return VideoManager;
})();
//# sourceMappingURL=VideoManager.js.map
