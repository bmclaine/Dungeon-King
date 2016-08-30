using UnityEngine;
using System.Collections;



    public class PlayerSpawner : MonoBehaviour 
    {
        public Transform position;

        void Start() {

            EntityManager.instance.player.transform.position = position.position;
        
        }
    }

