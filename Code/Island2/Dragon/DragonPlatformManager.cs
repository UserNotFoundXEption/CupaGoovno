using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DragonPlatformManager
{
    public void Init()
    {
        On.DragonLevelPlatformManager.Init += Init;
        On.DragonLevelPlatformManager.spawn_platforms += spawn_platforms;
    }

    public void Init(On.DragonLevelPlatformManager.orig_Init orig, DragonLevelPlatformManager self, LevelProperties.Dragon.Clouds properties)
    {
        self.properties = properties;
        self.toggleDelay = true;
        self.platforms = new List<DragonLevelCloudPlatform>();
        for (int i = 0; i < self.maxPlatforms; i++)
        {
            DragonLevelCloudPlatform dragonLevelCloudPlatform = UnityEngine.Object.Instantiate<DragonLevelCloudPlatform>(self.platformPrefab);
            dragonLevelCloudPlatform.gameObject.SetActive(false);
            dragonLevelCloudPlatform.transform.parent = self.transform;
            self.platforms.Add(dragonLevelCloudPlatform);
        }
        self.StartCoroutine(cloudsClear_cr(self));//new
        //self.StartCoroutine(self.spawn_platforms());
        //self.StartCoroutine(self.run_delay_cr());
    }

    private IEnumerator spawn_platforms(On.DragonLevelPlatformManager.orig_spawn_platforms orig, DragonLevelPlatformManager self)
    {
        List<string> positions = new List<string>(self.properties.cloudPositions);
        int mainIndex = UnityEngine.Random.Range(0, positions.Count);
        string[] positionString = positions[mainIndex].Split(new char[]
        {
        ','
        });
        int positionIndex = UnityEngine.Random.Range(0, positionString.Length);
        int platformIndex = 0;
        float platformWidth = self.platformPrefab.GetComponent<Renderer>().bounds.size.x / 2f;
        float waitTime = 0f;
        float position = 0f;
        //for (; ; )
        //{
        /*while (self.toggleDelay)
			{
				yield return null;
			}*/
        positionString = positions[mainIndex].Split(new char[]
        {
            ','
        });
        self.startPosition = ((!self.properties.movingRight) ? (640f + platformWidth) : (-640f - platformWidth));
        if (positionString[positionIndex][0] == 'D')
        {
            Parser.FloatTryParse(positionString[positionIndex].Substring(1), out waitTime);
        }
        else
        {
            string[] array = positionString[positionIndex].Split(new char[]
            {
                '-'
            });
            foreach (string s in array)
            {
                Parser.FloatTryParse(s, out position);
                self.platforms[platformIndex].transform.position = new Vector3(self.startPosition, 360f - position, 0f);
                self.platforms[platformIndex].gameObject.SetActive(true);
                self.platforms[platformIndex].GetProperties(self, self.properties);
                platformIndex = (platformIndex + 1) % self.platforms.Count;
            }
            waitTime = self.properties.cloudDelay;
        }
        yield return CupheadTime.WaitForSeconds(self, waitTime);
        if (positionIndex < positionString.Length - 1)
        {
            positionIndex++;
        }
        else if (positions.Count > 1)
        {
            positions.Remove(positions[mainIndex]);
            positionIndex = 0;
            mainIndex = UnityEngine.Random.Range(0, positions.Count);
        }
        else
        {
            positionIndex = 0;
            mainIndex = 0;
            positions = new List<string>(self.properties.cloudPositions);
        }
        //}
        yield break;
    }

    private IEnumerator cloudsClear_cr(DragonLevelPlatformManager self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 3f);
        self.StartCoroutine(self.spawn_platforms());
        yield return CupheadTime.WaitForSeconds(self, 3f);
        self.StartCoroutine(ChangeCloudsStopX(self, false));
        yield break;
    }

    public static IEnumerator ChangeCloudsStopX(DragonLevelPlatformManager self, bool wait)
    {
        if (wait)
        {
            yield return CupheadTime.WaitForSeconds(self, 5f);
        }
        foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in self.platforms)
        {
            DragonCloudPlatform.ChangeStopX(dragonLevelCloudPlatform);
        }
        yield break;
    }
}
