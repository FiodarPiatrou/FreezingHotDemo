using System;
using Enums;

namespace Utils
{
    [Serializable]
    public struct QuestStruct
    {
        public QuestState QuestState;
        public QuestsEnum Quests;
    }
}