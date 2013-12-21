/// <reference path="../definitions/jquery/jquery.d.ts" />
/// <reference path="../definitions/bootstrap/bootstrap.d.ts" />
/// <reference path="MediaFile.ts" />

interface Client {
    PhotoCount: number;
    FullName: string;
}

class PhotoSelector {

    private step = 1;
    private client;

    private availableTitle: JQuery;
    private selectedTitle: JQuery;
    private availableList: JQuery;
    private selectedList: JQuery;
    private title: JQuery;
    private okButton: JQuery;
    private loading: JQuery;
    private tab1: JQuery;
    private tab2: JQuery;
    private hash: JQuery;

    constructor(private dataUrl: string, private thumbUrl: string, private element: JQuery, private resource: any) {
        this.availableTitle = element.find('.available-title');
        this.selectedTitle = element.find('.selected-title');
        this.availableList = element.find('.available-items');
        this.selectedList = element.find('.selected-items');
        this.title = element.find('.modal-title');
        this.okButton = element.find('.ok-button');
        this.loading = element.find('.loading');
        this.tab1 = element.find('.tab1');
        this.tab2 = element.find('.tab2');
        this.hash = element.find('.hash');
    }

    refresh() {
        var availableLength = this.availableList.find('img').length;
        var selectedLength = this.selectedList.find('img').length;

        this.availableTitle.html(this.resource.AvailablePhotos + ' (' + availableLength + ')');
        this.selectedTitle.html(this.resource.SelectedPhotos + ' (' + selectedLength + ' ' + this.resource.Of + ' ' + this.client.PhotoCount + ')');
        $('.modal-title').html(this.resource.PhotosOf + ' <strong>' + this.client.FullName + '</strong>');
    }

    init() {
        var self = this;
        var data;

        this.availableList.on('click', 'img', function () {
            var $element = $(this);
            $element.fadeOut(300, () => {
                self.selectedList.append($element);
                self.refresh();
                $element.fadeIn(300);
            });
        });

        self.selectedList.on('click', 'img', function () {
            var $element = $(this);
            $element.fadeOut(300, () => {
                self.availableList.append($element);
                self.refresh();
                $element.fadeIn(300);
            });
        });

        self.okButton.on('click', () => {
            if (this.step == 1) {
                this.loading.show();

                data = {
                    hash: this.hash.val()
                };

                jQuery.ajax(this.dataUrl, {
                    data: data,
                    error: () => {
                        alert(this.resource.CouldNotFindAClientWithThisCode);
                        this.loading.hide();
                    },
                    success: client=> {
                        if (!client.AvailableMediaFiles) {
                            alert(client.Text);
                            this.element.modal('hide');
                            return;
                        }
                        this.client = client;
                        mediaFiles = client.AvailableMediaFiles;
                        for (i = 0; i < mediaFiles.length; i++) {
                            var $img = $(MediaFileManagerModule.thumbHtml(mediaFiles[i], this.thumbUrl, 240));
                            var $div = $('<div />');
                            //$div.append('<button>lupa</button>');
                            $div.append($img);
                            $div.data('mediaFile', mediaFiles[i]);
                            this.availableList.append($div);
                        }
                        this.refresh();

                        this.tab1.hide();
                        this.tab2.show();
                        this.step = 2;
                    }
                });
            } else if (this.step == 2) {
                if (confirm(this.resource.AreYouSureYouWanToSendThisPhotoSelection)) {
                    this.loading.show();

                    var $items = this.selectedList.find('div');

                    var confirmMessage = this.resource.YouSignAAlbumWith + this.client.PhotoCount
                        + this.resource.PhotosButYouAreTryingToSend + $items.length
                        + this.resource.PhotosItWillAddCost;

                    if ($items.length <= this.client.PhotoCount
                        || confirm(confirmMessage)) {
                        var mediaFiles = [];
                        for (var i = 0; i < $items.length; i++) {
                            mediaFiles.push($($items[i]).data('mediaFile'));
                        }
                        data = {
                            hash: this.hash.val(),
                            selectedMediaFilesJson: JSON.stringify(mediaFiles)
                        };

                        jQuery.ajax(this.dataUrl, {
                            data: data,
                            method: 'post',
                            error: () => {
                                alert(this.resource.CantSavePhotos);
                                this.loading.hide();
                            },
                            success: message=> {
                                alert(message.Text);
                                this.element.modal('hide');
                            }
                        });
                    }
                }
            }
        });
    }
}