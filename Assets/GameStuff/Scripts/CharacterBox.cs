using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace characterInterface
{

    public interface CHARACTERBOX
    {
        public void Initialization(int position, float xp);
        public bool isDeath();
        public void attack(List<CHARACTERBOX> characters);
        public void takeDamage(float damageToTake);
        public float getSpeed();
        public void setGameObject(GameObject gameObject);
        public GameObject getGameObject();
        public void deleteItself();
        public void changeAnimation(string newAnimation);
        public void addExperience(float value);
        public int getCharacterID(); //ID is what character in the list
        public void reset();
    }


    public class Character1 : CHARACTERBOX //This is a sample character that will be deleted later, these should be named when the characters are more developed
    {
        int id = 0; //Remember to change this to what it shows up in the list as following index rules



        float speed = 10;
        float totalHealth = 100; //Not sure if this is needed but just in case, can delete later if it's not
        float health = 100;
        float damage = 20;
        GameObject thisObject;
        string currentlyPlayingAnimation;
        int formationPosition = 0; //Positions can be 
                                   //1
                                   //2, 3
                                   //4, 5, 6
                                   //If we want more positions later than we can add them
                                   //Starts at 0 because it needs to be initialized into something
        float experience = 0; //xp that the character has, will be developed more later on


        public void Initialization(int position, float xp)
        {
            //this.thisObject = new GameObject(); //This won't be for a while I assume but create the sprite and place it where it should be
            this.formationPosition = position;
            this.currentlyPlayingAnimation = "Iddle";
            this.experience = xp;
        }

        public bool isDeath()
        {
            return (this.health <= 0);
        }

        public void attack(List<CHARACTERBOX> characters)
        {
            //Different types of attacks will be put in here, for now this will be just attack the first enemy
            characters[0].takeDamage(damage);
        }

        public void takeDamage(float damageToTake)
        {
            this.health -= damageToTake;
        }

        public float getSpeed()
        {
            return this.speed;
        }
        public void setGameObject(GameObject gameObject)
        {
            thisObject = gameObject;
        }

        public GameObject getGameObject()
        {
            return this.thisObject;
        }
        public void deleteItself()
        {
            UnityEngine.Object.Destroy(this.thisObject);
        }
        public void changeAnimation(string newAnimation)
        {
            currentlyPlayingAnimation = newAnimation;
            switch (newAnimation)
            {
                case "Attack":
                    this.getGameObject().GetComponent<Animator>().SetTrigger("Attack");
                    break;
                case "Iddle":
                    this.getGameObject().GetComponent<Animator>().SetTrigger("Iddle");
                    break;
                default:
                    Debug.Log("Hey something went wrong");
                    break;
            }
        }
        public void addExperience(float value)
        {
            this.experience += value;
        }
        public int getCharacterID()
        {
            return id;
        }

        public void reset()
        {
            this.health = this.totalHealth;
        }
    }

}


public class CharacterBox : MonoBehaviour
{
    //Characters have
    //health attack sounds animations speed 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
