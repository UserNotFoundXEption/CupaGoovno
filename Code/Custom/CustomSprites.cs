using Blender.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.U2D;
using UnityEngine;

namespace CupaGoovno;

public class CustomSprites
{
    public void Init()
    {
        On.AbstractMapCardIcon.setIcons += setIcons;
    }

    private void setIcons(On.AbstractMapCardIcon.orig_setIcons orig, AbstractMapCardIcon self, string iconPath, bool isGrey, string atlasName)
    {
        List<Sprite> list = new List<Sprite>();
        string fileName = Path.GetFileName(iconPath);

        Dictionary<string, string> newNameDic = new Dictionary<string, string>//new start
        {
            { "equip_icon_charm_hp1", "fury_heart" },
            { "equip_icon_charm_hp2", "cursed_heart" },
            { "equip_icon_charm_smoke-dash", "practise_mode" },
        };

        if (fileName.Substring(0, 5) == "charm")
        {
            GetPathsForCharms(fileName);
            UpdateSpritesCustom(self);
        }
        else if (fileName.Substring(0, 12) == "level_weapon")
        {
            GetPathsForWeapons(fileName);
            UpdateSpritesCustom(self);
        }
        else if (fileName.Substring(0, 11) == "level_super")
        {
            GetPathsForSupers(fileName);
            UpdateSpritesCustom(self);
        }

        else if (newNameDic.ContainsKey(fileName))
        {
            fileName = newNameDic[fileName];
            UpdateSpritesExisting(self, fileName);
        }

        else
        {//new end
            SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(atlasName);
            Sprite sprite = self.getSprite(cachedAsset, fileName);
            if (sprite != null)
            {
                list.Add(sprite);
            }
            for (int i = 1; i < 4; i++)
            {
                string arg = "_000";
                string fileName2 = Path.GetFileName(iconPath + arg + i);
                Sprite sprite2 = self.getSprite(cachedAsset, fileName2);
                if (!(sprite2 == null))
                {
                    list.Add(sprite2);
                }
            }
            self.normalIcons = list.ToArray();
            list.Clear();
            if (sprite != null)
            {
                list.Add(sprite);
            }
            for (int j = 1; j < 4; j++)
            {
                string arg2 = "_grey_000";
                string fileName3 = Path.GetFileName(iconPath + arg2 + j);
                Sprite sprite3 = self.getSprite(cachedAsset, fileName3);
                if (!(sprite3 == null))
                {
                    list.Add(sprite3);
                }
            }
            self.greyIcons = list.ToArray();
        }//new

        self.icons = !isGrey ? self.normalIcons : self.greyIcons;
        self.StopAllCoroutines();
        if (iconPath != WeaponProperties.GetIconPath(Weapon.None))
        {
            self.StartCoroutine(self.animate_cr());
        }
        else
        {
            self.SetIcon(self.icons[0]);
        }
    }

    private void GetPathsForCharms(string fileName)
    {
        int charmId = 0;
        for (int i = 0; i < EquipRegistries.Charms.GetValues().Count; i++)
        {
            if (EquipRegistries.Charms.GetNames()[i] == fileName)
            {
                charmId = i;
                break;
            }
        }
        atlas = EquipRegistries.Charms.GetValues()[charmId].AtlasPath;
        normalIcons = EquipRegistries.Charms.GetValues()[charmId].NormalIcons;
        greyIcons = EquipRegistries.Charms.GetValues()[charmId].GreyIcons;
    }

    private void GetPathsForWeapons(string fileName)
    {
        int weaponId = 0;
        for (int i = 0; i < EquipRegistries.Weapons.GetValues().Count; i++)
        {
            if (EquipRegistries.Weapons.GetNames()[i] == fileName)
            {
                weaponId = i;
                break;
            }
        }
        atlas = EquipRegistries.Weapons.GetValues()[weaponId].AtlasPath;
        normalIcons = EquipRegistries.Weapons.GetValues()[weaponId].NormalIcons;
        greyIcons = EquipRegistries.Weapons.GetValues()[weaponId].GreyIcons;
    }

    private void GetPathsForSupers(string fileName)
    {
        int superId = 0;
        for (int i = 0; i < EquipRegistries.Supers.GetValues().Count; i++)
        {
            if (EquipRegistries.Supers.GetNames()[i] == fileName)
            {
                superId = i;
                break;
            }
        }
        atlas = EquipRegistries.Supers.GetValues()[superId].AtlasPath;
        normalIcons = EquipRegistries.Supers.GetValues()[superId].NormalIcons;
        greyIcons = EquipRegistries.Supers.GetValues()[superId].GreyIcons;
    }

    private void UpdateSpritesCustom(AbstractMapCardIcon self)
    {
        List<Sprite> list = new List<Sprite>();
        SpriteAtlas cachedAsset = AssetLoader<UnityEngine.Object>.GetCachedAsset(atlas) as SpriteAtlas;
        for (int i = 0; i < normalIcons.Length; i++)
        {
            Sprite sprite = cachedAsset.GetSprite(normalIcons[i]);
            if (sprite != null)
            {
                list.Add(sprite);
            }
        }
        self.normalIcons = list.ToArray();
        list.Clear();
        for (int i = 0; i < greyIcons.Length; i++)
        {
            Sprite sprite = cachedAsset.GetSprite(greyIcons[i]);
            if (sprite != null)
            {
                list.Add(sprite);
            }
        }
        self.greyIcons = list.ToArray();
    }

    private void UpdateSpritesExisting(AbstractMapCardIcon self, string fileName)
    {
        List<Sprite> list = new List<Sprite>();
        SpriteAtlas cachedAsset = AssetLoader<UnityEngine.Object>.GetCachedAsset(CustomSprites.spriteAtlas) as SpriteAtlas;
        for (int i = 1; i < 4; i++)
        {
            Sprite sprite = cachedAsset.GetSprite(fileName + "_000" + i);
            if (sprite != null)
            {
                list.Add(sprite);
            }
        }
        self.normalIcons = list.ToArray();
        list.Clear();
        for (int i = 1; i < 4; i++)
        {
            Sprite sprite = cachedAsset.GetSprite(fileName + "_grey_000" + i);
            if (!(sprite == null))
            {
                list.Add(sprite);
            }
        }
        self.greyIcons = list.ToArray();
    }

    private string atlas;
    private string[] normalIcons;
    private string[] greyIcons;

    public static string spriteAtlas = "CupaGoovno:cupagoovno\\sprites";
}
