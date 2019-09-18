using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doom3d
{
    public static class ImageLibrary
    {
        public static Image OpenEyedCat =
            new Image(new[] {
                " /\\_/\\ ",
                "(='X'=)",
                "(\")_(\")"});

        public static Image ClosedEyedCat =
            new Image(new[] {
                " /\\_/\\ ",
                "(= X =)",
                "(\")_(\")"});

        public static Image OpenEyedMouse =
            new Image(new[] {
                "()-()",
                " \\\"/ ",
                "  `  "});

        public static Image ClosedEyedMouse =
            new Image(new[] {
                "()-()",
                " \\ / ",
                "  `  "});
    }
}
