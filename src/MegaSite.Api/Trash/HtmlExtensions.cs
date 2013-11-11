using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dongle.System;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Resources;

namespace MegaSite.Api.Trash
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Option(this HtmlHelper helper, string name)
        {
            return new MvcHtmlString(Options.Instance.Get(name));
        }

        public static MvcHtmlString FieldsInputs(this HtmlHelper helper, IEnumerable<Field> fields)
        {
            var html = "";
            foreach (var field in fields.OrderBy(f => f.Order))
            {
                html += "<div class=\"editor-label\">" + helper.Label(field.Name) + "</div>";

                html += "<div class=\"editor-field\">";

                var value = field.Value ?? "";
                switch (field.Type)
                {
                    case FieldType.Text:
                        html += helper.TextBox(field.Name, value);
                        break;
                    case FieldType.Color:
                        html += helper.TextBox(field.Name, value, new { @class = "colorpicker" });
                        break;
                    case FieldType.Boolean:
                        html += "<div class=\"switch\" data-on-label=\"" + Resource.Yes + "\" data-off-label=\"" + Resource.No + "\"><input name=\"" +
                                 field.Name + "\" type=\"checkbox\" " + ((string.IsNullOrEmpty(field.Value) || field.Value.ToLower() == "false") ? "" : "checked=\"checked\" ") + "/></div>";
                        break;
                    case FieldType.RichText:
                        html += helper.TextArea(field.Name, value, new { @class = "ckeditor" });
                        break;
                    case FieldType.SelectList:
                        html += helper.DropDownList(field.Name, new SelectList(field.SelectList ?? new List<string>(), value), Resource.Select);
                        break;
                    case FieldType.DateTime:
                        html += helper.TextBox(field.Name, value, new { @class = "datetimepicker" });
                        break;
                    case FieldType.Date:
                        html += helper.TextBox(field.Name, value, new { @class = "datepicker" });
                        break;
                    case FieldType.Number:
                        html += helper.TextBox(field.Name, value, new { @type = "number" });
                        break;
                    case FieldType.Email:
                        html += helper.TextBox(field.Name, value, new { @type = "email" });
                        break;
                    case FieldType.Media:
                        html += helper.TextBox(field.Name, value, new { @class = "media-picker-control" });
                        break;
                    case FieldType.Location:
                        html += helper.TextBox(field.Name, value);
                        break;
                }
                html += "</div>" + helper.ValidationMessage(field.Name);
            }
            return new MvcHtmlString(html);
        }

        public static HtmlString Thumbnail(this UrlHelper helper, MediaFile mediaFile, int? width = null, int? height = null, bool crop = false, string title = null, string @class = "", bool force = false)
        {
            if (mediaFile == null)
            {
                return new HtmlString("");
            }
            title = String.IsNullOrEmpty(title) ? "" : ("title=\"" + title + "\"");
            string url;
            if (mediaFile.ThumbUrl != null && !force)
            {
                url = mediaFile.ThumbUrl;
            }
            else
            {
                width = width ?? 450;
                height = height ?? 450;

                var u1 = String.IsNullOrEmpty(mediaFile.Url) ? null : mediaFile.Url.ToBase64();
                url = helper.Content("~/" + MediaFileManager.Instance.GetThumbPath(mediaFile.FileName, width.Value, height.Value, crop));
                if (u1 != null)
                {
                    url += "?url=" + u1;
                }
            }
            return new HtmlString("<img class=\"" + @class + "\" src=\"" + url + "\" " + title + "/>");
        }

        public static HtmlString DefaultThumbnail(this UrlHelper helper, MediaFile mediaFile)
        {
            return Thumbnail(helper, mediaFile, 240, 240, true, null, "img-rounded");
        }

        public static MvcHtmlString DateTimeSpan(this HtmlHelper htmlHelper, DateTime? datetime)
        {
            if (!datetime.HasValue)
            {
                return new MvcHtmlString("");
            }
            var tagBuilder = new TagBuilder("span")
            {
                InnerHtml = datetime.Value.ToFriendlyString()
            };
            tagBuilder.MergeAttribute("title", datetime.ToString());
            tagBuilder.AddCssClass("wdatetimespan");
            return new MvcHtmlString(tagBuilder.ToString());
        }
    }
}
