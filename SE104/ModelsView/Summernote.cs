using Microsoft.AspNetCore.Mvc;

namespace SE104.ModelsView
{
    public class Summernote 
    {
        public Summernote(string iDEditor, bool loadLibrary = true)
        {
            IDEditor = iDEditor;
            LoadLibrary = loadLibrary;
        }
        public string IDEditor { get; set; }
        public string GetIDEditor() => IDEditor;

        public bool LoadLibrary { get; set; }

        public int height { get; set; } = 1200;

        public string toolbar { get; set; } = @"
            [
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['font', ['strikethrough', 'superscript', 'subscript']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['insert', ['link', 'picture', 'video', 'elfinder']],
                ['height', ['height']]
            ]";
    }
}
