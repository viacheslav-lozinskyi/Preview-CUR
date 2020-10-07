
using System;
using System.IO;
using System.Windows.Forms;

namespace resource.preview
{
    public class CUR : cartridge.AnyPreview
    {
        protected override void _Execute(atom.Trace context, string url)
        {
            if (File.Exists(url))
            {
                var a_Context = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read);
                {
                    var a_Context1 = new Cursor(a_Context);
                    {
                        var a_Size = GetProperty(NAME.PROPERTY.LIMIT_PREVIEW_SIZE);
                        {
                            a_Size = Math.Min(a_Size, a_Context1.Size.Height / CONSTANT.OUTPUT_PREVIEW_ITEM_HEIGHT);
                            a_Size = Math.Max(a_Size, CONSTANT.OUTPUT_PREVIEW_MIN_SIZE);
                        }
                        for (var i = 0; i < a_Size; i++)
                        {
                            __Send(context, NAME.PATTERN.PREVIEW, 1, "", "");
                        }
                    }
                    {
                        context.
                            SetState(NAME.STATE.FOOTER).
                            Send(NAME.PATTERN.ELEMENT, 1, "[[Size]]: " + a_Context1.Size.Width.ToString() + " x " + a_Context1.Size.Height.ToString());
                        {
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[File Name]]", url);
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[File Size]]", (new FileInfo(url).Length).ToString());
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Width]]", a_Context1.Size.Width.ToString());
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Height]]", a_Context1.Size.Height.ToString());
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Hotspot]].X", ((int)a_Context1.HotSpot.X).ToString());
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Hotspot]].Y", ((int)a_Context1.HotSpot.Y).ToString());
                            __Send(context, NAME.PATTERN.VARIABLE, 2, "[[Raw Format]]", "CUR");
                        }
                    }
                }
                {
                    a_Context.Dispose();
                }
            }
            else
            {
                context.
                    SendError(1, "[[File not found]]");
            }
        }

        private static void __Send(atom.Trace context, string pattern, int level, string name, string value)
        {
            context.
                Clear().
                SetValue(value).
                Send(pattern, level, name);
        }
    };
}
