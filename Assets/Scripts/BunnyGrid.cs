using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BunnyGrid : MonoBehaviour
{
    // Grid size
    public static int w = 10;
    public static int h = 12;


    public GameObject[] gems;

     
    public static ArrayList _myGems;
    public LineRenderer lr;
    public ParticleSystem ps1;
    public ParticleSystem ps2;

    public Text text;
	public GameObject SFXButton;
	private bool sound = true;
    int NumberOfFound = 0;

 

    GameObject selectedGem = null;
	private bool playingFalling = false;
 
    int solved = 0;
    private void Start()
    {
        _myGems = new ArrayList();
         

        ps1.GetComponent<Renderer>().sortingLayerID = lr.sortingLayerID;
        ps1.GetComponent<Renderer>().sortingOrder = lr.sortingOrder;
        ps2.GetComponent<Renderer>().sortingLayerID = lr.sortingLayerID;
        ps2.GetComponent<Renderer>().sortingOrder = lr.sortingOrder;


        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
                // Spawn
                spawnAt(x * 0.8f, y * 0.6f);
        }
        StartCoroutine(StartSolving());
    }

    private IEnumerator StartSolving()
    {
       
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            SolveMatch();
         
        // Debug.Log("solve");
   

        }
     }


public void reload()
    {
        Debug.Log("reloading");
        //   SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        solved = 0;
        LeanTween.cancelAll();
        foreach (GameObject go in _myGems)
        {
            DestroyImmediate(go);
           
        }
        _myGems.Clear();

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
                // Spawn
                spawnAt(x * 0.8f, y * 0.6f);
        }
    }


    public GameObject FindSelection(Vector2 position)
    {
        
        ContactFilter2D cf = new ContactFilter2D();
        BoxCollider2D[] results = new BoxCollider2D[5];
        int count = Physics2D.OverlapPoint(position, cf, results);
      
        BoxCollider2D hitCollider = null;

        if (count > 0)
        {
            hitCollider = results[0];
        }



        if (hitCollider)
        {
          
                 return hitCollider.gameObject; 
         
        }
        return null;
    }

	public void SoundOnOff() {
		sound = !sound;
		int activeIndex = sound == true ? 0 : 1;
		int inactiveIndex = sound == true ? 1 : 0;
		Image img = SFXButton.transform.GetChild(inactiveIndex).gameObject.GetComponent<Image>();
		img.enabled = false;
		img = SFXButton.transform.GetChild(activeIndex).gameObject.GetComponent<Image>();
		img.enabled = true;
        AudioListener.volume  = sound == true ? 1 : 0;
	}

    // Update is called once per frame
    void Update () {
 
        // SolveMatch(); 
        foreach (GameObject go in _myGems)
        {
           
                
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedGem = FindSelection(mousePosition);
            if(selectedGem != null)
            {
              
                lr.SetPosition(0, mousePosition);
                lr.SetPosition(1, mousePosition);
 
            }

        }

        if(Input.GetMouseButton(0) && selectedGem != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          
            float rainbowdist = Vector2.Distance(mousePosition,   lr.GetPosition(0)  );
            if(rainbowdist < 1.0f)
            {
                lr.SetPosition(1, mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
           
            GameObject toswitch = FindSelection(lr.GetPosition(1));
            float distance = 1000;
            if(selectedGem!= null && toswitch!=null )
            {
                 distance = Vector2.Distance(selectedGem.transform.position, toswitch.transform.position);
            }

            lr.SetPosition(0, new Vector3(-10,-10,-0));
            lr.SetPosition(1, new Vector3(-10, -10, -0));
     
            if (distance < 1.0f)
            {

                if (toswitch != null)
                {
                    if (toswitch != selectedGem)
                    {
                         Vector3 remember = toswitch.transform.position;

                       
             
                        ps1.transform.position = toswitch.transform.position;
                        ps1.Play();

                        ps2.transform.position = selectedGem.transform.position;
                        ps2.Play();


						/*
                        var col = ps1.colorOverLifetime;
                        col.enabled = true;

                        Gradient grad = new Gradient();
                        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

                        col.color = grad;
                        */
						PlayRandomSound prs = toswitch.GetComponent<PlayRandomSound>();
						if(prs) {
							prs.PlayMoving();
						}
                        LeanTween.alpha(toswitch, 0f, 0.1f).setEase(LeanTweenType.easeInCirc).setOnComplete(() => {
                            toswitch.transform.position = selectedGem.transform.position;
                            LeanTween.alpha(toswitch, 1f, 0.1f).setEase(LeanTweenType.easeInCirc);

                        });
                        LeanTween.alpha(selectedGem, 0f, 0.1f).setEase(LeanTweenType.easeInCirc).setOnComplete(() => {
                            selectedGem.transform.position = remember;
                            LeanTween.alpha(selectedGem, 1f, 0.1f).setEase(LeanTweenType.easeInCirc);
                              selectedGem = null;
                        });
               
                    
                         

                    }
                }
               
             
            }
            else
            {
               
                if(selectedGem !=null)
                {
                    selectedGem = null;

                }
            }

          
        }

    }
    
    void spawnAt(float x, float y) {
        int index = Random.Range(0, gems.Length);
        GameObject go = Instantiate(gems[index], new Vector2(x, y), Quaternion.identity);
       
        _myGems.Add(go);
        Animator anim = go.GetComponent<Animator>();

        if (anim)
        {
            anim.enabled = false;

            StartCoroutine(ReEnable(anim));
          
        }
    }

    private IEnumerator ReEnable(Animator anim)
    {
       
            yield return new WaitForSeconds(Random.Range(0.0f,10.2f));
       
       
    if(anim)
        {
            anim.enabled = true;
            anim.speed = Random.Range(0.2f,1.0f);
        
        }

    }
 
 
    public static float AngleDir(Vector3 A, Vector3 B)
    {
        return -A.x * B.y + A.y * B.x;
    }

    public bool sameType(GameObject selected ,GameObject other)
    {
        return selected.GetComponent<SpriteRenderer>().sprite == other.GetComponent<SpriteRenderer>().sprite;
    }


    void removeObjectPretty(GameObject go)
    {
        go.tag = "Untagged"; //tweak the range down below to change when they dissapear
        LeanTween.alpha(go, 0f, Random.Range(0.1f,0.4f)).setEase(LeanTweenType.easeInCirc).setOnComplete( () => {
            _myGems.Remove(go);
            DestroyImmediate(go);
        });
         
    }
 

    public bool FindNextTo(GameObject selected, int direction)
    {
 
        foreach (GameObject go in _myGems)
        {
            if(go!=selected)
                {
            float distance = Vector2.Distance(selected.transform.position, go.transform.position);

                float maxDistance = 0.1f;
                if(direction == 0)
                {
                    maxDistance = 1.1f;
                }
                else if (direction == 1)
                {
                    maxDistance = 0.6f;
                }



                if( distance< maxDistance )
            {
                //print("Distance: " +distance);
            
            float angle = AngleDir(selected.transform.position, go.transform.position);
          //   print("angledir: +" + angle);
            if (angle > 0)
             {
                       //  print("right");
                        float distancexory = 0;
                        if(direction == 0)
                        {
                            distancexory = Mathf.Abs(selected.transform.position.y - go.transform.position.y);
                        }
                        else
                        {
                            distancexory = Mathf.Abs(selected.transform.position.x - go.transform.position.x);
                        }

                        if (distancexory < 0.4f) //tweaka denna för att hitta mer oalignade
                        {
    
                            if (sameType(selected,go) == true )
                            {
                                NumberOfFound++;
                              //      print("also same type: "+NumberOfFound);
                                FindNextTo(go, direction);

                                if (NumberOfFound > 1)
                                {
 
                                    removeObjectPretty(go);
                                    removeObjectPretty(selected);
                                }
                                return true;
                            }
                        }

                     
              }
 
            }
        }
        }
        return false;
    }


    void SolveMatch()
    {
       // GameObject[] bunnies = GameObject.FindGameObjectsWithTag("Bunny");  
       // Debug.Log("bunnies.length: " + bunnies.Length);
      
     
        foreach (GameObject go in _myGems)
        {
            if (!go) continue;

            NumberOfFound = 0;
          FindNextTo(go, 0);

          //  Debug.Log("NumberOfFound: " + NumberOfFound);

            if (NumberOfFound > 1)
            {
           
                removeObjectPretty(go);
                solved+=NumberOfFound+1;
             
                text.text = ""+solved;
            }
        
        }

  
  
        foreach (GameObject go in _myGems)
        {
            NumberOfFound = 0;
            FindNextTo(go, 1);


            if (NumberOfFound > 1)
            {
                removeObjectPretty(go);
                solved += NumberOfFound + 1;
      
                text.text = "" + solved;
               
            }

        }
 

    }
 
    
}
