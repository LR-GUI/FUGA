using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool[] levelClear;

    public bool unlockBadEnding;
    public bool unlockGoodEnding;
    public bool unlock;

    public SaveData(GameMaster gm) {
        unlockBadEnding = gm.unlockBadEnding;
        unlockGoodEnding = gm.unlockGoodEnding;
        unlock = gm.unlock;

        levelClear = new bool[16];
        levelClear[0] = gm.levelClear[0];
        levelClear[1] = gm.levelClear[1];
        levelClear[2] = gm.levelClear[2];
        levelClear[3] = gm.levelClear[3];
        levelClear[4] = gm.levelClear[4];
        levelClear[5] = gm.levelClear[5];
        levelClear[6] = gm.levelClear[6];
        levelClear[7] = gm.levelClear[7];
        levelClear[8] = gm.levelClear[8];
        levelClear[9] = gm.levelClear[9];
        levelClear[10] = gm.levelClear[10];
        levelClear[11] = gm.levelClear[11];
        levelClear[12] = gm.levelClear[12];
        levelClear[13] = gm.levelClear[13];
        levelClear[14] = gm.levelClear[14];
        levelClear[15] = gm.levelClear[15];
    }
}
