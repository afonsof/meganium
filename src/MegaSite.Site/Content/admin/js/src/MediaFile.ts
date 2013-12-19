/// <reference path="../definitions/jquery/jquery.d.ts" />
/// <reference path="../definitions/bootstrap/bootstrap.d.ts" />
/// <reference path="../definitions/uploadfive/uploadfive.d.ts" />
/// <reference path="../definitions/hacks.d.ts" />

module MediaFileManagerModule {
    export function init($elements: JQuery, options: IMediaFileManagerOptions): JQuery {
        return $elements.each(function () {
            var $this = $(this);
            if (!$this.data('mediafilemanager')) {
                $this.data('mediafilemanager', new MediaFileManager($this, options));
            }
        });
    }

    export enum MediaFileManagerType {
        Picker,
        Album
    }

    export interface IMediaFileManagerOptions {
        rootUrl: string;
        uploadUrl: string;
        listItemsUrl: string;
        thumbUrl: string;
        thumbSize: number;
        selectText: string;
        changeText: string;
        removeText: string;
        okText: string;
        uploadText: string;
        type: MediaFileManagerType;
    }

    export interface IMediaFile {
        FileName: string;
        ThumbUrl: string;
        Url: string;
    }

    export function thumbHtml(mediaFile: IMediaFile, thumbUrl: string, thumbSize: number): string {
        if (mediaFile == null) {
            return "";
        }
        var url: string;
        if (mediaFile.ThumbUrl != null) {
            url = mediaFile.ThumbUrl;
        }
        else {
            var u1 = (!mediaFile.Url) ? null : base64.encode(mediaFile.Url);
            url = thumbUrl + '/' + mediaFile.FileName + '-' + thumbSize + 'x' + thumbSize + '-crop.jpg';
            if (u1 != null) {
                url += "?url=" + u1;
            }
        }
        return '<img class="img-rounded" src="' + url + '" />';
    }

    export class MediaFileManager {
        private _currentControl: JQuery;
        private _element: JQuery;

        private _rootUrl: string;
        private _uploadUrl: string;
        private _listItemsUrl: string;
        private _thumbUrl: string;

        private _thumbSize: number;
        private _selectText: string;
        private _changeText: string;
        private _removeText: string;
        private _okText: string;
        private _uploadText: string;

        private _type: MediaFileManagerType;

        constructor(field: JQuery, options: IMediaFileManagerOptions) {
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

            if (this._type == MediaFileManagerType.Picker) {
                this._element = this.pickerHtml();
                jQuery(document.body).append(this._element);

                this._element.on('dblclick', '.item', function () {
                    self.selectItem(jQuery(this));
                });

                this._element.on('click', '.item', function () {
                    self.checkItem($(this));
                });

                this._element.on('click', '.btn-ok', () => this.onOkClick());
                jQuery.getJSON(this._listItemsUrl, (data: Array<IMediaFile>) => this.fillGallery(data));
                this.registerField(field);
            }
            else if (this._type == MediaFileManagerType.Album) {
                this._element = this.bodyHtml();
                this._element.addClass('album');
                this._element.data('field', field);
                this._currentControl = this._element;
                field.hide();

                if (field.val()) {
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

        //Event Handlers
        private onOkClick(): void {
            this.selectItem(this._element.find('.selected'));
        }

        //Html generators

        private pickerHtml(): JQuery {
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
        }

        private bodyHtml(): JQuery {
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
        }

        private itemHtml(mediaFile: IMediaFile): JQuery {
            var item = $('<div class="span2 item">' + thumbHtml(mediaFile, this._thumbUrl, this._thumbSize) + '</div>');
            if (this._type == MediaFileManagerType.Album) {
                item.append('<button type="button" class="btn btn-danger">×</button>');
            }
            item.data('mediaFile', mediaFile);
            return item;
        }

        private galleryListHtml(data: Array<IMediaFile>): JQuery[] {
            var items = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                items.push(this.itemHtml(item));
            }
            return items;
        }

        //Actions
        private checkItem(item: JQuery): void {
            this._element.find('.item').removeClass('selected');
            item.addClass('selected');
        }

        private selectItem(item: JQuery): void {
            this.fillCurrentControl(item.data('mediaFile'));
            this._element.modal('hide');
        }

        private fillGallery(data: Array<IMediaFile>): void {
            jQuery(this._element.find('.media-picker-gallery').append(this.galleryListHtml(data)));
        }

        private fillCurrentControl(mediaFile): void {
            if (this._type == MediaFileManagerType.Picker) {
                if (mediaFile) {
                    this._currentControl.data('field').val(JSON.stringify(mediaFile));
                    this._currentControl.html("");
                    this._currentControl.append(thumbHtml(mediaFile, this._thumbUrl, this._thumbSize));
                    this._currentControl.append($('<div class="btn btn-change call-modal">' + this._changeText + '...</div>'));
                    this._currentControl.append($('<div class="btn btn-remove btn-danger">' + this._removeText + '...</div>'));
                }
                else {
                    this._currentControl.html('<a href="javascript:void(0)" class="btn-empty call-modal img-rounded"><i class="icon icon-picture" style="font-size: 90px;"></i><br />' + this._selectText + '...</div>');
                }
            }
            else {
                var items: IMediaFile[] = new Array<IMediaFile>();
                this._element.find('.item').each(function () {
                    items.push($(this).data('mediaFile'));
                });
                this._currentControl.data('field').val(JSON.stringify(items));
            }
        }

        private setupUploader(element: JQuery, uploadUrl: string) {
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
                'formData': { 'test': 'something' },
                'queueID': element.find('.queue').attr('id'),
                'uploadScript': uploadUrl,
                'simUploadLimit': 1,
                'onUploadComplete': function (file, data: string) {
                    var obj: IMediaFile = jQuery.parseJSON(data);
                    if (obj.FileName != null) {
                        var item = $(self.itemHtml(obj));
                        jQuery(element.find('.media-picker-gallery').prepend(item));
                        if (self._type == MediaFileManagerType.Picker) {
                            self.checkItem(item);
                        }
                        else if (self._type == MediaFileManagerType.Album) {
                            self.fillCurrentControl(null);
                        }
                    }
                }
            });
        }

        public registerField(field: JQuery) {
            var self = this;
            field.hide();
            if (field.data("mediapicker")) {
                return;
            }
            field.data("mediapicker", true);
            var control = $('<div class="media-file-picker-control"></div>');
            control.data("field", field);

            var value = field.val();
            var mediaFile = null;
            if (value) {
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
        }
    }
}

(function ($) {
    $.fn.mediaFileManager = function (method, ...args: any[]) {
        if (MediaFileManagerModule[method]) {
            return MediaFileManagerModule[method].apply(MediaFileManagerModule, [this].concat(args));
        } else if (typeof method === 'object' || !method) {
            return MediaFileManagerModule.init.call(MediaFileManagerModule, this, arguments[0]);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.mediaFileManager');
        }
    };
})(jQuery);