﻿using System.Collections;
using UnityEngine;

namespace Etienne.Feedback {
    [System.Serializable]
    public class Wait : GameFeedback {
        public Wait() => color = new Color(255 / 255f, 153 / 255f, 51 / 255f);

        [SerializeField] private bool realtime;
        [SerializeField] private float timer;

        public override IEnumerator Execute(GameEvent gameEvent) {
            if(realtime) yield return new WaitForSecondsRealtime(timer);
            else yield return new WaitForSeconds(timer);
        }

        public override string ToString() => $"Wait {timer}s{(realtime ? ", in realtime" : "")}";
    }
}