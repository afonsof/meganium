var MediaFileManagerModule;
(function (MediaFileManagerModule) {
    function init($elements, options) {
        return $elements.each(function () {
            var $this = $(this);
            if(!$this.data('mediafilemanager')) {
                $this.data('mediafilemanager', new MediaFileManager($this, options));
            }
        });
    }
    MediaFileManagerModule.init = init;
    (function (MediaFileManagerType) {
        MediaFileManagerType._map = [];
        MediaFileManagerType._map[0] = "Picker";
        MediaFileManagerType.Picker = 0;
        MediaFileManagerType._map[1] = "Album";
        MediaFileManagerType.Album = 1;
    })(MediaFileManagerModule.MediaFileManagerType || (MediaFileManagerModule.MediaFileManagerType = {}));
    var MediaFileManagerType = MediaFileManagerModule.MediaFileManagerType;
    var MediaFileManager = (function () {
        function MediaFileManager(field, options) {
            var _this = this;
            this._rootUrl = options.rootUrl ? options.rootUrl : null;
            this._uploadUrl = options.uploadUrl ? options.uploadUrl : null;
            this._listItemsUrl = options.listItemsUrl ? options.listItemsUrl : null;
            this._thumbUrl = options.thumbUrl ? options.thumbUrl : null;
            this._thumbSize = options.thumbSize ? options.thumbSize : 240;
            this._selectText = options.selectText ? options.selectText : 'Select media';
            this._changeText = options.changeText ? options.changeText : 'Change media';
            this._removeText = options.removeText ? options.removeText : 'Remove media';
            this._okText = options.okText ? options.okText : 'Ok';
            this._uploadText = options.uploadText ? options.uploadText : 'Upload';
            this._type = options.type;
            var self = this;
            if(this._type == MediaFileManagerType.Picker) {
                this._element = this.pickerHtml();
                jQuery(document.body).append(this._element);
                this._element.on('dblclick', '.item', function () {
                    self.selectItem(jQuery(this));
                });
                this._element.on('click', '.item', function () {
                    self.checkItem($(this));
                });
                this._element.on('click', '.btn-ok', function () {
                    return _this.onOkClick();
                });
                jQuery.getJSON(this._listItemsUrl, function (data) {
                    return _this.fillGallery(data);
                });
                this.registerField(field);
            } else if(this._type == MediaFileManagerType.Album) {
                this._element = this.bodyHtml();
                this._element.addClass('album');
                this._element.data('field', field);
                this._currentControl = this._element;
                field.hide();
                if(field.val()) {
                    this.fillGallery(JSON.parse(field.val()));
                }
                this._element.on('click', '.btn-danger', function () {
                    $(this).closest('.item').fadeOut(500, function () {
                        $(this).remove();
                        self.fillCurrentControl(null);
                    });
                });
                this._element.insertAfter(field);
            }
            this.setupUploader(this._element, this._uploadUrl);
        }
        MediaFileManager.prototype.onOkClick = function () {
            this.selectItem(this._element.find('.selected'));
        };
        MediaFileManager.prototype.pickerHtml = function () {
            var element = $('<div class="modal hide fade media-picker-modal">\
                    <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>\
                        <h3>' + this._selectText + '</h3>\
                    </div>\
                    <div class="modal-body"></div>\
                    <div class="modal-footer">\
                        <button class="btn btn-primary btn-ok">' + this._okText + '</button >\
                    </div>\
                </div>');
            element.find('.modal-body').append(this.bodyHtml());
            return element;
        };
        MediaFileManager.prototype.bodyHtml = function () {
            var rand = Math.floor(Math.random() * 100);
            var element = $('<div class="row-fluid">\
                    <div class="span3">\
                        <div id="media-picker-uploader">\
                            <form>\
                                <input class="file-upload" name="file" type="file" multiple="true" />\
                                <div class="queue" id="queue' + rand + '"></div>\
                            </form>\
                        </div>\
                    </div>\
                    <div class="span9 row-fluid media-picker-gallery" >\
                    </div>\
                </div>');
            return element;
        };
        MediaFileManager.prototype.itemHtml = function (mediaFile) {
            var item = $('<div class="span2 item">' + this.thumbHtml(mediaFile) + '</div>');
            if(this._type == MediaFileManagerType.Album) {
                item.append('<button type="button" class="btn btn-danger">×</button>');
            }
            item.data('mediaFile', mediaFile);
            return item;
        };
        MediaFileManager.prototype.thumbHtml = function (mediaFile) {
            if(mediaFile == null) {
                return "";
            }
            var url;
            if(mediaFile.ThumbUrl != null) {
                url = mediaFile.ThumbUrl;
            } else {
                var u1 = (!mediaFile.Url) ? null : base64.encode(mediaFile.Url);
                url = this._thumbUrl + '/' + mediaFile.FileName + '-' + this._thumbSize + 'x' + this._thumbSize + '-crop.jpg';
                if(u1 != null) {
                    url += "?url=" + u1;
                }
            }
            return '<img class="img-rounded" src="' + url + '" />';
        };
        MediaFileManager.prototype.galleryListHtml = function (data) {
            var items = [];
            for(var i = 0; i < data.length; i++) {
                var item = data[i];
                items.push(this.itemHtml(item));
            }
            return items;
        };
        MediaFileManager.prototype.checkItem = function (item) {
            this._element.find('.item').removeClass('selected');
            item.addClass('selected');
        };
        MediaFileManager.prototype.selectItem = function (item) {
            this.fillCurrentControl(item.data('mediaFile'));
            this._element.modal('hide');
        };
        MediaFileManager.prototype.fillGallery = function (data) {
            jQuery(this._element.find('.media-picker-gallery').append(this.galleryListHtml(data)));
        };
        MediaFileManager.prototype.fillCurrentControl = function (mediaFile) {
            if(this._type == MediaFileManagerType.Picker) {
                if(mediaFile) {
                    this._currentControl.data('field').val(JSON.stringify(mediaFile));
                    this._currentControl.html("");
                    this._currentControl.append(this.thumbHtml(mediaFile));
                    this._currentControl.append($('<div class="btn btn-change call-modal">' + this._changeText + '...</div>'));
                    this._currentControl.append($('<div class="btn btn-remove btn-danger">' + this._removeText + '...</div>'));
                } else {
                    this._currentControl.html('<a href="javascript:void(0)" class="btn-empty call-modal img-rounded"><i class="icon icon-picture" style="font-size: 90px;"></i><br />' + this._selectText + '...</div>');
                }
            } else {
                var items = new Array();
                this._element.find('.item').each(function () {
                    items.push($(this).data('mediaFile'));
                });
                this._currentControl.data('field').val(JSON.stringify(items));
            }
        };
        MediaFileManager.prototype.setupUploader = function (element, uploadUrl) {
            var self = this;
            var uploader = element.find('.file-upload');
            uploader.uploadifive({
                'auto': true,
                'removeCompleted': true,
                'fileType': 'image',
                'buttonClass': 'btn',
                'height': '20',
                'width': 'auto',
                'buttonText': '<i class="icon-cloud-upload"></i> ' + self._uploadText + '...',
                'formData': {
                    'test': 'something'
                },
                'queueID': element.find('.queue').attr('id'),
                'uploadScript': uploadUrl,
                'simUploadLimit': 1,
                'onUploadComplete': function (file, data) {
                    var obj = jQuery.parseJSON(data);
                    if(obj.FileName != null) {
                        var item = $(self.itemHtml(obj));
                        jQuery(element.find('.media-picker-gallery').prepend(item));
                        if(self._type == MediaFileManagerType.Picker) {
                            self.checkItem(item);
                        } else if(self._type == MediaFileManagerType.Album) {
                            self.fillCurrentControl(null);
                        }
                    }
                }
            });
        };
        MediaFileManager.prototype.registerField = function (field) {
            var self = this;
            field.hide();
            if(field.data("mediapicker")) {
                return;
            }
            field.data("mediapicker", true);
            var control = $('<div class="media-file-picker-control"></div>');
            control.data("field", field);
            var value = field.val();
            var mediaFile = null;
            if(value) {
                mediaFile = jQuery.parseJSON(value);
            }
            self._currentControl = control;
            self.fillCurrentControl(mediaFile);
            control.on('click', '.call-modal', function () {
                self._currentControl = $(this).parent();
                self._element.modal('show');
            }).on('click', '.btn-remove', function () {
                $(this).parent().data('field').val('');
                self._currentControl = control;
                self.fillCurrentControl(null);
            }).on('mouseover', function () {
                $(this).find('.btn').show();
            }).on('mouseout', function () {
                $(this).find('.btn').hide();
            });
            control.insertAfter(field);
        };
        return MediaFileManager;
    })();    
})(MediaFileManagerModule || (MediaFileManagerModule = {}));
(function ($) {
    $.fn.mediaFileManager = function (method) {
        var args = [];
        for (var _i = 0; _i < (arguments.length - 1); _i++) {
            args[_i] = arguments[_i + 1];
        }
        if(MediaFileManagerModule[method]) {
            return MediaFileManagerModule[method].apply(MediaFileManagerModule, [
                this
            ].concat(args));
        } else if(typeof method === 'object' || !method) {
            return MediaFileManagerModule.init.call(MediaFileManagerModule, this, arguments[0]);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.mediaFileManager');
        }
    };
})(jQuery);
//@ sourceMappingURL=MediaFile.js.map
