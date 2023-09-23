using PdfSharp.Fonts;

namespace BlazorAppExport.Helpers;

public class FileFontResolver : IFontResolver
{
    public string DefaultFontName => throw new NotImplementedException();

    public byte[] GetFont(string faceName)
    {
        using (var ms = new MemoryStream())
        {
            using (var fs = File.Open(faceName, FileMode.Open))
            {
                fs.CopyTo(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Arial", StringComparison.CurrentCultureIgnoreCase))
        {
            if (isBold && isItalic)
            {
                return new FontResolverInfo("Fonts/arialbi.ttf");
            }
            else if (isBold)
            {
                return new FontResolverInfo("Fonts/arialbd.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo("Fonts/ariali.ttf");
            }
            else
            {
                return new FontResolverInfo("Fonts/arial.ttf");
            }
        }
        if (familyName.Equals("Times New Roman", StringComparison.CurrentCultureIgnoreCase))
        {
            if (isBold && isItalic)
            {
                return new FontResolverInfo("Fonts/timesbi.ttf");
            }
            else if (isBold)
            {
                return new FontResolverInfo("Fonts/timesbd.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo("Fonts/timesi.ttf");
            }
            else
            {
                return new FontResolverInfo("Fonts/times.ttf");
            }
        }
        if (familyName.Equals("Tahoma", StringComparison.CurrentCultureIgnoreCase))
        {
            if (isBold && isItalic)
            {
                return new FontResolverInfo("Fonts/tahoma.ttf");
            }
            else if (isBold)
            {
                return new FontResolverInfo("Fonts/tahomabd.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo("Fonts/tahoma.ttf");
            }
            else
            {
                return new FontResolverInfo("Fonts/tahoma.ttf");
            }
        }
        if (familyName.Equals("Verdana", StringComparison.CurrentCultureIgnoreCase))
        {
            if (isBold && isItalic)
            {
                return new FontResolverInfo("Fonts/verdanaz.ttf");
            }
            else if (isBold)
            {
                return new FontResolverInfo("Fonts/verdanab.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo("Fonts/verdanai.ttf");
            }
            else
            {
                return new FontResolverInfo("Fonts/verdana.ttf");
            }
        }
        if (familyName.Equals("Courier New", StringComparison.CurrentCultureIgnoreCase))
        {
            if (isBold && isItalic)
            {
                return new FontResolverInfo("Fonts/courbi.ttf");
            }
            else if (isBold)
            {
                return new FontResolverInfo("Fonts/courbd.ttf");
            }
            else if (isItalic)
            {
                return new FontResolverInfo("Fonts/couri.ttf");
            }
            else
            {
                return new FontResolverInfo("Fonts/cour.ttf");
            }
        }
        return null;
    }
}