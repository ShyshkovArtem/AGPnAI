using UnityEngine;
using UnityEngine.UI;


// Helper extensions for UI elements.

public static class ImageExtensions
{
    // Sets the sprite on the Image, or hides it if sprite is null.
    public static void SetSpriteOrHide(this Image img, Sprite sprite)
    {
        if (img == null) return;
        img.sprite = sprite;
        img.enabled = sprite != null;
    }
}