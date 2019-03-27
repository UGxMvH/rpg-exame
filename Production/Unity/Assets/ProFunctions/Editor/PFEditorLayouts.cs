using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProFunctions
{
    public static class PFEditorLayouts
    {

        public static GUIStyle labelWithColor()
        {
            GUIStyle s = new GUIStyle(EditorStyles.label);
            s.normal.textColor = Color.green;

            return s;
        }

        public static GUIStyle IconButton()
        {
            // create a style based on the default label style
            GUIStyle myStyle = new GUIStyle(GUI.skin.button);
            // do whatever you want with this style, e.g.:
            myStyle.padding = new RectOffset(0,0,0,0);
            myStyle.imagePosition = ImagePosition.ImageOnly;

            return myStyle;
        }
  
    }
}
