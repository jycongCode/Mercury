using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Mercury_MM{
    [CustomEditor(typeof(test),true)]
    public class testUI : Editor
    {
        protected test myTest;

        private void OnEnable()
        {
            myTest = (test)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
