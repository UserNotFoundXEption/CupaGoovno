using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.U2D;
using UnityEngine;

namespace CupaGoovno;

public class Localization2
{
    public void Init()
    {
        //On.Localization.Find_int += Find;
        //On.Localization.Find_string += Find;
        //On.WeaponProperties.GetDisplayName_Charm += GetDisplayName;
        On.LocalizationHelper.ApplyTranslation_TranslationElement += ApplyTranslation;
    }

    public static string GetDisplayName(On.WeaponProperties.orig_GetDisplayName_Charm orig, Charm charm)
    {
        Plugin.Log(charm.ToString());
        TranslationElement translationElement = Localization.Find(charm.ToString() + "_name");
        if (translationElement == null)
        {
            return "ERROR";
        }
        return translationElement.translation.text;
    }

    public static TranslationElement Find(On.Localization.orig_Find_string orig, string key)
    {
        Plugin.Log("key: " + key);
        for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
        {
            if (Localization._instance.m_TranslationElements[i].key == key)
            {
                return Localization._instance.m_TranslationElements[i];
            }
        }
        return null;
    }

    public static TranslationElement Find(On.Localization.orig_Find_int orig, int id)
    {
        Plugin.Log("id: " + id);
        for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
        {
            if (Localization._instance.m_TranslationElements[i].id == id)
            {
                return Localization._instance.m_TranslationElements[i];
            }
        }
        return null;
    }

    public void ApplyTranslation(On.LocalizationHelper.orig_ApplyTranslation_TranslationElement orig, LocalizationHelper self, TranslationElement translationElement)
    {
        if (!self.isInit)
        {
            self.Init();
        }
        self.currentLanguage = Localization.language;
        if (self.currentLanguage == (Localization.Languages)(-1) || translationElement == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(translationElement.key))
        {
            return;
        }
        Localization.Translation translation = translationElement.translation;
        if (string.IsNullOrEmpty(translation.text))
        {
            translation = Localization.Translate(translationElement.key);
        }
        string text = translation.text;
        if (text != null)
        {
            text = text.Replace("\\n", "\n");
        }
        if (text != null && text.Contains("{") && text.Contains("}"))
        {
            if (self.subTranslations != null)
            {
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    for (int i = 0; i < self.subTranslations.Length; i++)
                    {
                        if (text.Contains("{" + self.subTranslations[i].key + "}"))
                        {
                            flag = true;
                            if (self.subTranslations[i].dontTranslate)
                            {
                                text = text.Replace("{" + self.subTranslations[i].key + "}", self.subTranslations[i].value);
                            }
                            else
                            {
                                Localization.Translation translation2 = Localization.Translate(self.subTranslations[i].value);
                                if (string.IsNullOrEmpty(translation2.text))
                                {
                                    text = text.Replace("{" + self.subTranslations[i].key + "}", self.subTranslations[i].value);
                                }
                                else
                                {
                                    text = text.Replace("{" + self.subTranslations[i].key + "}", translation2.text);
                                }
                            }
                        }
                    }
                }
            }

            string[] array = text.Split(new char[]
            {
                '{'
            });
            if (array.Length > 1)
            {
                string[] array2 = array[1].Split(new char[]
                {
                    '}'
                });
                if (array2.Length > 1)
                {
                    string text2 = array2[0];
                    Localization.Translation translation3 = Localization.Translate(text2);
                    if (!string.IsNullOrEmpty(translation3.text))
                    {
                        text = text.Replace("{" + text2 + "}", translation3.text);
                    }
                }
            }
        }
        if (self.textComponent != null)
        {
            self.textComponent.text = text;
            self.textComponent.enabled = !string.IsNullOrEmpty(text);
            //if (translation.hasCustomFont)
            if (translation.fonts != null && translation.hasCustomFont)//new
            {
                self.textComponent.font = translation.fonts.font;
            }
            else if (Localization.Instance.fonts[(int)self.currentLanguage][(int)translationElement.category].fontType != FontLoader.FontType.None)
            {
                self.textComponent.font = Localization.Instance.fonts[(int)self.currentLanguage][(int)translationElement.category].font;
            }
            //self.textComponent.fontSize = ((translation.fonts.fontSize <= 0) ? self.initialFontSize : translation.fonts.fontSize);
            if (translation.fonts != null)//new start
            {
                self.textComponent.fontSize = ((translation.fonts.fontSize <= 0) ? self.initialFontSize : translation.fonts.fontSize);
            }
            else
            {
                self.textComponent.fontSize = self.initialFontSize;
            }//new end
        }
        if (self.textMeshProComponent != null)
        {
            self.textMeshProComponent.text = text;
            self.textMeshProComponent.enabled = !string.IsNullOrEmpty(text);
            //self.textMeshProComponent.characterSpacing = translation.fonts.charSpacing;
            if(translation.fonts == null)//new start
            {
                self.textMeshProComponent.characterSpacing = 0;
            }
            else
            {
                self.textMeshProComponent.characterSpacing = translation.fonts.charSpacing;
            }//new end
            //if (translation.hasCustomFontAsset)
            if (translation.fonts != null && translation.hasCustomFontAsset)//new
            {
                self.textMeshProComponent.font = translation.fonts.fontAsset;
            }
            else
            {
                self.textMeshProComponent.font = Localization.Instance.fonts[(int)self.currentLanguage][(int)translationElement.category].fontAsset;
            }
            //self.textMeshProComponent.fontSize = ((translation.fonts.fontAssetSize <= 0f) ? self.initialFontAssetSize : translation.fonts.fontAssetSize);
            self.textMeshProComponent.fontSize = ((translation.fonts != null && translation.fonts.fontAssetSize > 0f) ? translation.fonts.fontAssetSize : self.initialFontAssetSize);//new
        }
        if (self.spriteRendererComponent != null)
        {
            Sprite sprite;
            if (translation.hasSpriteAtlasImage)
            {
                SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
                sprite = cachedAsset.GetSprite(translation.spriteAtlasImageName);
            }
            else
            {
                sprite = translation.image;
            }
            self.spriteRendererComponent.sprite = sprite;
            self.spriteRendererComponent.enabled = false;
            self.spriteRendererComponent.enabled = (sprite != null);
        }
        if (self.imageComponent != null)
        {
            Sprite sprite2;
            if (translation.hasSpriteAtlasImage)
            {
                SpriteAtlas cachedAsset2 = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
                sprite2 = cachedAsset2.GetSprite(translation.spriteAtlasImageName);
            }
            else
            {
                sprite2 = translation.image;
            }
            self.imageComponent.sprite = sprite2;
            self.imageComponent.enabled = false;
            self.imageComponent.enabled = (sprite2 != null);
        }
    }
}
