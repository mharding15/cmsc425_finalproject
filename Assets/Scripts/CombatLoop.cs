﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLoop : MonoBehaviour
{
    // this list contains instances of the inner class "Person" I made below to keep track of things for that character
	private List<Person> characters;
    // this list contains the actual GameObjects associated with the characters, so can use it to call their methods and stuff
    private List<GameObject> objects;
    // this int keeps track of whose turn it is (the index of the character in the above lists)
	private int current;

    // for testing
    private int count;

    void Start()
    {
        characters = new List<Person>();
        objects = new List<GameObject>();
    	current = -1;
        count = 0;

    	GameObject[] chars = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject obj in chars){
            GetCharacter(obj);
        }
    	
        // can add some kind of delay here if we want so it doesn't just immediately start combat, could be kind of confusing
        Next();

    }

    // I wasn't sure how to differentiate between the characters after getting them by the "Character" tag, so I do it by seeing what script is attached
    void GetCharacter(GameObject obj)
    {
        Person p = new Person();
        if (obj.GetComponent<Bruno>()){
            print("This is Bruno");
            p.name = "Bruno";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Bruno.speed + Bruno.reaction; 
            p.initiative = obj.GetComponent<Bruno>().speed;
            p.isEnemy = obj.GetComponent<Bruno>().isEnemy;
        } else if (obj.GetComponent<Erika>()){
            print("This is Erika");
            p.name = "Erika";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Erika.speed + Erika.reaction;
            p.isEnemy = obj.GetComponent<Erika>().isEnemy;
        } else if (obj.GetComponent<Maria>()){
            print("This is Maria");
            p.name = "Maria";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Maria.speed + Maria.reaction; 
            p.initiative = obj.GetComponent<Maria>().speed;
            p.isEnemy = obj.GetComponent<Maria>().isEnemy;
        } else if (obj.GetComponent<Panos>()){
            print("This is Panos");
            p.name = "Panos";
            p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Panos>().speed + obj.GetComponent<Panos>().reaction;
            p.isEnemy = obj.GetComponent<Panos>().isEnemy;
        } else if (obj.GetComponent<Ganfaul>()){
            print("This is Ganfaul");
            p.name = "Ganfaul";
            p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Ganfaul>().speed + obj.GetComponent<Ganfaul>().reaction;
            p.isEnemy = obj.GetComponent<Ganfaul>().isEnemy;
        } else if (obj.GetComponent<Nightshade>()){
            print("This is Nightshade");
            p.name = "Nightshade";
            p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Nightshade>().speed + obj.GetComponent<Nightshade>().reaction;
            p.isEnemy = obj.GetComponent<Nightshade>().isEnemy;
        } else if (obj.GetComponent<Warrok>()){
            print("This is Warrok");
            p.name = "Warrok";
            p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Warrok>().speed + obj.GetComponent<Warrok>().reaction; 
            p.isEnemy = obj.GetComponent<Warrok>().isEnemy;

        } else if (obj.GetComponent<Mulok>()){
            print("This is Mulok");
            p.name = "Mulok";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Mulok.speed + Mulok.reaction; 
            p.initiative = obj.GetComponent<Mulok>().speed;
            p.isEnemy = obj.GetComponent<Mulok>().isEnemy;

        } else if (obj.GetComponent<Vurius>()){
            print("This is Vurius");
            p.name = "Vurius";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Vurius.speed + Vurius.reaction; 
            p.initiative = obj.GetComponent<Vurius>().speed;
            p.isEnemy = obj.GetComponent<Vurius>().isEnemy;
        } else if (obj.GetComponent<Zontog>()){
            print("This is Zontog");
            p.name = "Zontog";
            //p.initiative = UnityEngine.Random.Range(1, 20) + Zontog.speed + Zontog.reaction; 
            p.isEnemy = obj.GetComponent<Zontog>().isEnemy;
        } else {
            print("It's none of them???");
        }
        print("Initiative is: " + p.initiative);
        AddToList(p, obj);
    }

    void AddToList(Person p, GameObject o)
    {
        int i;
        for (i = 0; i < characters.Count; i++){
            if (p.initiative >= characters[i].initiative){
                break;
            }
        }
        characters.Insert(i, p);
        objects.Insert(i, o);
    }

    // this method should be called whenever a character is done with their turn (so last action has been taken). It will then move to the next character's turn.
    public void Next()
    {
        current = (current + 1) % characters.Count;
        count++;
        if (count < 10){
            print("In Next...and current is: " + current);

            if (characters[current].isEnemy){
                print("In Next...and current was an enemy");
                Walk();
            } else {
                print("In Next...and current was a friend");
                if (characters[current].name == "Bruno"){
                    print("Bruno should be moving...");
                    objects[current].GetComponent<Bruno>().Move();
                    Next();
                } else if (characters[current].name == "Maria"){
                    print("Maria should be moving...");
                    objects[current].GetComponent<Maria>().Move();
                    Next();
                }
            }
        }
    }

    void Walk(){
        print("In Walk and name is: " + characters[current].name);
        if (characters[current].name == "Mulok"){
            print("In Walk and Mulok should be moving...");
            objects[current].GetComponent<Mulok>().Move();
            Next();
        } else if (characters[current].name == "Vurius"){
            print("In Walk and Virius should be moving...");
            objects[current].GetComponent<Vurius>().Move();
            Next();
        }
    }

    // private void MakeDecision()
    // {
    // 	int closestEnemy = FindClosestOpponent(current);
    // 	int weakestEnemy = FindWeakestOpponent(current);

    // 	if (characters[current].health < 5){
    // 		LowHealth();
    // 	} else {
    // 		HighHealth();
    // 	}
    // }

    // private void LowHealth()
    // {
    // 	if (characters[current].isEngaged){
    // 		// try to disengage
    // 	} else {
    // 		// take potion
    // 	}
    // }

    // private void HighHealth()
    // {
    // 	if (distance(characters[current], characters[closestEnemy]) < meleeAttackDist){
    // 		// melee attack that character
    // 	} else {
    // 		CheckRangedAttack();
    // 	}
    // }

    // private void CheckRangedAttack()
    // {
    // 	if (distance(characters[current], characters[closestEnemy]) < meleeAttackDist){
    // 		// do ranged attack
    // 	} else {
    // 		MakeMove();
    // 	}
    // }

    // private void MakeMove()
    // {
    // 	// with 1/2 probability
    // 		// move toward closestEnemy
    // 	// with 1/2 probablity
    // 		// move toward weakestEnemy
    // }
    
    // // *** Helpers (not part of the tree) *** //
    // private int FindClosestEnemy()
    // {

    // }

    // private int FindWeakestEnemy()
    // {

    // }

    void PrintList()
    {
        foreach (Person p in characters){
            print("The person is: " + p.name + ", and my initiative is: " + p.initiative);
        }
    }

    public class Person
    {
        public int initiative {set; get;}
        public string name {set; get;}
        public bool isEnemy {set; get;}
    }
}
