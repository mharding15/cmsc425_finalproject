using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLoop : MonoBehaviour
{
    public Text humanTurnText;
    public Text humanHPText; 
    public Text humanModeText;
    public Text humanTargetText;

    public Text aiTurnText;
    public Text aiHPText;

    public Text turnResultText;
    public Text gameResultText;

    public GameObject humanPanel;
    public GameObject aiPanel;
    public GameObject turnResultPanel;

    // this list contains instances of the inner class "Person" I made below to keep track of things for that character
	private List<Person> characters;
    // this list contains the actual GameObjects associated with the characters, so can use it to call their methods and stuff
    private List<GameObject> objects;
    // this int keeps track of whose turn it is (the index of the character in the above lists)
    private List<Unit> units;
	private int current, countdown;

    public GameObject mainCamera;
    private CameraController cameraController;

    // for testing
    private int count;

    void Start()
    {
        characters = new List<Person>();
        objects = new List<GameObject>();
        units = new List<Unit>();
    	current = -1;
        count = 0;
        countdown = 3;

        cameraController = mainCamera.GetComponent<CameraController>();

        GameObject[] chars = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject obj in chars){
            GetCharacter(obj);
        }

        DeactivatePanels();
    	
        Countdown();
    }

    public int getCurrentInt()
    {
        return current;
    }

    void DeactivatePanels()
    {
        humanPanel.SetActive(false);
        aiPanel.SetActive(false);
        turnResultPanel.SetActive(false);
    }

    // I wasn't sure how to differentiate between the characters after getting them by the "Character" tag, so I do it by seeing what script is attached
    void GetCharacter(GameObject obj)
    {
        Person p = new Person();
        if (obj.GetComponent<Bruno>()){
            print("This is Bruno");
            p.name = "Bruno";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Bruno>().speed + obj.GetComponent<Bruno>().reaction; 
            p.initiative = obj.GetComponent<Bruno>().speed;
            p.isEnemy = obj.GetComponent<Bruno>().isEnemy;
        } else if (obj.GetComponent<Erika>()){
            print("This is Erika");
            p.name = "Erika";
            p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Erika>().speed + obj.GetComponent<Erika>().reaction;
            p.initiative = obj.GetComponent<Erika>().speed;
            //p.isEnemy = obj.GetComponent<Erika>().isEnemy;
        } else if (obj.GetComponent<Maria>()){
            print("This is Maria");
            p.name = "Maria";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Maria>().speed + obj.GetComponent<Maria>().reaction; 
            p.initiative = obj.GetComponent<Maria>().speed;
            p.isEnemy = obj.GetComponent<Maria>().isEnemy;
        } else if (obj.GetComponent<Panos>()){
            print("This is Panos");
            p.name = "Panos";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Panos>().speed + obj.GetComponent<Panos>().reaction;
            p.initiative = obj.GetComponent<Panos>().speed;
            p.isEnemy = obj.GetComponent<Panos>().isEnemy;
        } else if (obj.GetComponent<Ganfaul>()){
            print("This is Ganfaul");
            p.name = "Ganfaul";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Ganfaul>().speed + obj.GetComponent<Ganfaul>().reaction;
            p.initiative = obj.GetComponent<Ganfaul>().speed;
            p.isEnemy = obj.GetComponent<Ganfaul>().isEnemy;
        } else if (obj.GetComponent<Nightshade>()){
            print("This is Nightshade");
            p.name = "Nightshade";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Nightshade>().speed + obj.GetComponent<Nightshade>().reaction;
            p.initiative = obj.GetComponent<Nightshade>().speed;
            p.isEnemy = obj.GetComponent<Nightshade>().isEnemy;
        } else if (obj.GetComponent<Warrok>()){
            print("This is Warrok");
            p.name = "Warrok";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Warrok>().speed + obj.GetComponent<Warrok>().reaction; 
            p.initiative = obj.GetComponent<Warrok>().speed;
            p.isEnemy = obj.GetComponent<Warrok>().isEnemy;

        } else if (obj.GetComponent<Mulok>()){
            print("This is Mulok");
            p.name = "Mulok";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Mulok>().speed + obj.GetComponent<Mulok>().reaction; 
            p.initiative = obj.GetComponent<Mulok>().speed;
            p.isEnemy = obj.GetComponent<Mulok>().isEnemy;

        } else if (obj.GetComponent<Vurius>()){
            print("This is Vurius");
            p.name = "Vurius";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Vurius>().speed + obj.GetComponent<Vurius>().reaction; 
            p.initiative = obj.GetComponent<Vurius>().speed;
            p.isEnemy = obj.GetComponent<Vurius>().isEnemy;
        } else if (obj.GetComponent<Zontog>()){
            print("This is Zontog");
            p.name = "Zontog";
            //p.initiative = UnityEngine.Random.Range(1, 20) + obj.GetComponent<Zontog>().speed + obj.GetComponent<Zontog>().reaction; 
            p.initiative = obj.GetComponent<Zontog>().speed;
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
        units.Insert(i, GetUnit(o));
    }

    void Countdown()
    {
        if (countdown == 0){
            // Setup some things
            foreach (Unit unit in units)
            {
                GameObject indicator;

                if (unit.isEnemy)
                    indicator = Manager.Instance.enemyUnitIndicator;
                else
                    indicator = Manager.Instance.friendlyUnitIndicator;

                GameObject inicatorObject = Instantiate(indicator, unit.transform.position, Quaternion.identity, unit.transform) as GameObject;
            }

                gameResultText.text = "";
            Next(.25f);
        } else {
            gameResultText.text = "" + countdown;
            countdown--;
            StartCoroutine(DelayCountdown(1f));
        }
    }

    public void Next()
    {
        StartCoroutine(Delay(0.5f));
    }

    public void Next(float delay)
    {
        print("888 should be called when character is dead");
        StartCoroutine(Delay(delay));
    }

    // this method should be called whenever a character is done with their turn (so last action has been taken). It will then move to the next character's turn.
    public void NextContinued()
    {

        // If the game is not over, go to the next player
        if (!GameOverCheck()){

            // need to reset the target that it says in the ui
            SetTargetText("None");

            int previous = current;
            current = (current + 1) % units.Count;
            print("1111 current is: " + current);

            // need to unset the character who was current last time in their GameObject's script
            if (previous > -1){
                units[previous].isCurrent = false;
            }
            units[current].isCurrent = true;

            // if the current character is down (but not dead, or they would be deleted from the lists)
            if (units[current].hp <= 0){
                Next(.01f);
            }

            // switch cameras

            cameraController.setCameraLockableObject(objects[current]);
            units[current].path = null;
            units[current].movableTiles = new MovableTileFinder(Manager.Instance.map, (int)(objects[current].transform.position.x),
                (int)(objects[current].transform.position.z),
                units[current].speed).solve();
            Manager.Instance.SetMovableTilePreview(units[current].movableTiles);
            Manager.Instance.SetAttackableTilePreview(units[current]);
            Manager.Instance.SetSelectedUnitIndicator(objects[current].transform);

            count++;
            // just don't want to get caught in an infinte loop or something.
            if (count < 200){
                // this is an AI character, need to make it's decisions for it
                print("101010101 Combat Loop line 203 :: The current # is : " + current);
                // if (characters[current].isEnemy){
                //     print("444 In Next...and current was an enemy, character: " + objects[current].name);
                //     aiTurnText.text = characters[current].name + "'s Turn";
                //     aiHPText.text = "HP: " + units[current].hp;
                //     print("555 WHY is it now setting this panel to active???");
                //     aiPanel.SetActive(true);
                //     humanPanel.SetActive(false);
                //     AttackingDecisions();
                // } else {
                //     humanTurnText.text = characters[current].name + "'s Turn";
                //     humanHPText.text = "HP: " + units[current].hp;
                //     humanModeText.text = "Mode: None";

                //     aiPanel.SetActive(false);
                //     humanPanel.SetActive(true);
                // }
                humanTurnText.text = characters[current].name + "'s Turn";
                humanHPText.text = "HP: " + units[current].hp;
                humanModeText.text = "Mode: None";

                aiPanel.SetActive(false);
                humanPanel.SetActive(true);
            }
        } 
        
    }

    void DeactivateCameras()
    {
        for (int i = 0; i < objects.Count; i++){
            if (objects[i].transform.Find("Camera") != null){
                GameObject cam = objects[i].transform.Find("Camera").gameObject;
                if (cam != null && i != current){
                    cam.SetActive(false);
                }
            }
        }
    }

    // *** The Decision "Tree" (not like any tree I've seen before, but close enough) *** //

    // void MakeDecision()
    // {
    //     // if this character is about to die and has healing potions, then take one
    //     if (units[current].hp <= 5 && units[current].healingPotionCount > 0){
    //         print("The character: " + objects[current].name + " decides to take a healing potion");
    //         units[current].TakePotion();
    //     } else {
    //         AttackingDecisions();
    //     }
    // }

    void AttackingDecisions()
    {
        FindClosestOpponent();
        int closestOpponentIdx = characters[current].closestEnemy; 
        float closestEnemyDist = characters[current].closestEnemyDist;

        print("&&& For character: " + objects[current].name + ", the closest enemy is: " + objects[closestOpponentIdx].name + " and is " + closestEnemyDist + " away.");

        if (closestEnemyDist < units[current].meleeRange){
            print("&&& so gonna hit them with a Melee attack");
            units[current].target = objects[closestOpponentIdx];
            units[current].targetUnit = units[closestOpponentIdx];

            units[current].EnterMeleeMode();
        } else if (units[current].longRange > 0f && closestEnemyDist < units[current].longRange){
            print("&&& so gonna hit them with a long range");
            units[current].target = objects[closestOpponentIdx];
            units[current].targetUnit = units[closestOpponentIdx];
            if (!objects[current].name.Equals("Erika")){
                RangedUnit runit = GetRangedUnit(objects[current]);
                runit.EnterRangedMode();
            } else {
                objects[current].GetComponent<Erika>().EnterRangedMode();
            }
        } else {
            print("&&& so gonna move closer to that enemy");
            units[current].target = objects[closestOpponentIdx];
            units[current].targetUnit = units[closestOpponentIdx];
            units[current].path = new List<Vector3>();
            units[current].path.Add(objects[closestOpponentIdx].transform.position);
            units[current].EnterMoveMode();
        }
    }

    // *** Decision Making Helpers *** //

    void FindClosestOpponent()
    {
        // should be virually infinite
        float minDist = 10000f;
        int minIdx = -1;
        Unit currentUnit = units[current];
        for (int i = 0; i < characters.Count; i++){
            // need to make sure the enemy is not already dead
            if (i != current && units[i].hp > 0 && (units[current].isEnemy != units[i].isEnemy)){
                float dist = Distance(objects[current].transform.position, objects[i].transform.position);
                if (dist < minDist){
                    minDist = dist;
                    minIdx = i;
                }
            }
        }
        characters[current].closestEnemy = minIdx;
        characters[current].closestEnemyDist = minDist;
    }

    void FindClosestAlly()
    {
        float minDist = 10000f;
        int minIdx = -1;
        Unit currentUnit = units[current];
        for (int i = 0; i < characters.Count; i++){
            if (i != current && units[i].hp > 0 && units[current].isEnemy == units[i].isEnemy){
                float dist = Distance(objects[current].transform.position, objects[i].transform.position);
                if (dist < minDist){
                    minDist = dist;
                    minIdx = i;
                }
            }
        }
        characters[current].closestFriend = minIdx;
    }

    float Distance(Vector3 p1, Vector3 p2)
    {
        float x_diff = p1.x - p2.x;
        float z_diff = p1.z - p2.z;
        float d_sq = x_diff * x_diff + z_diff * z_diff;
        return Mathf.Sqrt(d_sq);
    }

    // *** Utility functions *** //

    bool GameOverCheck(){

        int friendsRemaining = 0, enemiesRemaining = 0;
        for (int i = 0; i < units.Count; i++){
            if (units[i].hp > 0){
                if (units[i].isEnemy){
                    enemiesRemaining++;
                } else {
                    friendsRemaining++;
                }
            }
        }
        if (friendsRemaining == 0){
            SetGameResultText("You Lose!");
            return true;
        } else if (enemiesRemaining == 0){
            SetGameResultText("You Win!");
            return true;
        }

        return false;
    }

    Unit GetUnit(GameObject obj)
    {
        Unit unit = null;

        if (obj.name == "Bruno"){
            unit = obj.GetComponent<Bruno>();
        } else if (obj.name == "Erika"){
            unit = obj.GetComponent<Erika>();
        } else if (obj.name == "Maria"){
            unit = obj.GetComponent<Maria>();
        }else if (obj.name == "Panos") {
            unit = obj.GetComponent<Panos>();
        } else if (obj.name == "Ganfaul"){
            unit = obj.GetComponent<Ganfaul>();
        } else if (obj.name == "Nightshade"){
            unit = obj.GetComponent<Nightshade>();
        } else if (obj.name == "Warrok"){
            unit = obj.GetComponent<Warrok>();
        } else if (obj.name == "Mulok"){
            unit = obj.GetComponent<Mulok>();
        } else if (obj.name == "Vurius"){
            unit = obj.GetComponent<Vurius>();
        } else if (obj.name == "Zontog"){
            unit = obj.GetComponent<Zontog>();
        }

        return unit;
    }

    RangedUnit GetRangedUnit(GameObject obj)
    {
        RangedUnit runit = null;

        if (obj.name == "Panos") {
            runit = obj.GetComponent<Panos>();
        }  else if (obj.name == "Nightshade"){
            runit = obj.GetComponent<Nightshade>();
        }  else if (obj.name == "Vurius"){
            runit = obj.GetComponent<Vurius>();
        } 

        return runit;
    }

    public GameObject GetCurrentObject()
    {
        return objects[current];
    }

    public GameObject GetObjectByName(string name)
    {
        int i;
        for (i = 0; i < characters.Count; i++){
            if (name.Equals(characters[i].name)){
                break;
            }
        }
        if (i == characters.Count){
            print("The name was not found???");
            return null;
        }
        return objects[i];
    }

    public Unit GetCurrentUnit()
    {
        return units[current];
    }

    public string GetCurrentName()
    {
        return characters[current].name;
    }

    public void SetDying(string name)
    {
        for (int i = 0; i < characters.Count; i++){
            if (name.Equals(characters[i].name)){
                characters[i].isDying = true;
            }
        }
    }

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
        public bool isDying {set; get;}
        public int closestEnemy {set; get;}
        public float closestEnemyDist {set; get;}
        public int closestFriend {set; get;}
    }

    public void SetGameResultText(string text)
    {
        gameResultText.text = text;
        aiPanel.SetActive(false);
        turnResultPanel.SetActive(false);
    }

    public void SetModeText(string text)
    {
        humanModeText.text = "Mode: " + text;
    }

    public void SetTurnResultText(string text)
    {
        turnResultPanel.SetActive(true);
        turnResultText.text = text;
        StartCoroutine(DelayForResultText(2f));
    }

    public void SetTargetText(string text)
    {
        humanTargetText.text = "Target: " + text;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        NextContinued();
    }

    IEnumerator DelayForResultText(float time)
    {
        yield return new WaitForSeconds(time);
        turnResultPanel.SetActive(false);
    }

    IEnumerator DelayCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        Countdown();
    }

    public List<Unit> GetUnits()
    {
        return units;
    }
}
