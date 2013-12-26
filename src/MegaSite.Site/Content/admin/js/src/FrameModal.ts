class FrameModal {
    private removeElement(element: HTMLElement) {
        element.parentElement.removeChild(element);
    }

    constructor() {
        var _this = this;
        var dialogs = document.getElementsByClassName('iframe-dialog');
        for (var i = 0; i < dialogs.length; i++) {
            dialogs[i].addEventListener("click", function () {
                var dialogId = this.attributes['data-dialog-id'];
                var iframe = document.createElement('iframe');
                iframe.id = dialogId.value;
                iframe.style.width = "100%";
                iframe.style.height = "100%";
                iframe.style.position = "fixed";
                iframe.style.top = '0';
                iframe.style.left = '0';
                iframe.style.zIndex = '99999';
                iframe.src = this.attributes['data-url'].value;
                iframe.style.display = 'none';

                var overlay = document.createElement('div');
                overlay.style.width = "100%";
                overlay.style.height = "100%";
                overlay.style.position = "fixed";
                overlay.style.top = '0';
                overlay.style.left = '0';
                overlay.style.zIndex = '99999';
                overlay.style.backgroundColor = "rgba(0,0,0,0.5)";

                iframe.onload = function () {                    
                    overlay.style.display = 'none';
                    iframe.style.display = 'block';
                    this.contentWindow.show(()=> {
                        _this.removeElement(iframe);
                        _this.removeElement(overlay);
                    });
                };
                document.body.appendChild(overlay);
                document.body.appendChild(iframe);
            });
        }
    }
}
var frameModal = new FrameModal();