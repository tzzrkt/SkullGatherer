using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class skullcounter : MonoBehaviour
{
    public GameObject[] skulls;
    public int amt;
    public int col;
    public int percent;
    public bool touched;
    public addfor enemy;

    public float minSpeed;
    public float maxSpeed;
    public float speed;
    public float catched;
    public bool win;
    public bool lose;

    public AudioSource amb;
    public AudioSource enem;
    public Transform charact;
    public Transform enemie;
    public float max;
    public float min;
    public float dist;

    public Light luz;
    public AudioSource asrc;
    public AudioClip point;
    public int pointss;
    public int counter;
    public GameObject start;
    public GameObject end;
    // Start is called before the first frame update
    void Start()
    {
        touched = false;
        Invoke("Findskulls", 10f);
    }

    public void Win()
    {
        win = true;
    }
    public void Lose()
    {
        lose = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Findskulls()
    {
        skulls = GameObject.FindGameObjectsWithTag("skull");
        amt = skulls.Length;
        start.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        if (pointss < col)
        {
            asrc.clip = point;
            asrc.Play();
            pointss = col;
        }

        if (amt > 0 || col > 1)
        {
            catched = col / (amt / percent);
            speed = Mathf.Lerp(minSpeed, maxSpeed, catched);
            enemy.maxSpeed = speed;
            enemy.force = speed;

            if (col > (amt / percent)) { Win(); }
        }
        if (amt == 0 || col == 0)
        {
            enemy.maxSpeed = 0f;
            enemy.force = 0f;
        }
        if (touched) { Lose(); }

        dist = Vector3.Distance(charact.position, enemie.position);
        if (dist >= max) { amb.volume = 1f; enem.volume = 0f; }
        else if (dist < max) { amb.volume = 0f; enem.volume = 1f; }

        if (win)
        {
            luz.intensity++;
            counter++;


            if (counter == 30)
            {
                end.SetActive(true);
                Invoke("Quit", 5f);
            }
        }

        // else if (dist <= max && dist >= min) { amb.spatialBlend = Mathf.Lerp(0f, 1f, dist); enem.spatialBlend = Mathf.Lerp(1f, 0f, dist); }

    }
}
