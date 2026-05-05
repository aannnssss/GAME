using UnityEngine;
using System.Collections;

public class TunnelExit : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject rocks;
    public FinishLineTrigger finishTrigger;

    private bool playerInside = false;
    private bool exploded = false;

    void Update()
    {
        if (playerInside && !exploded && Input.GetKeyDown(KeyCode.E))
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (gm != null && gm.CanEscape())
            {
                Explode();
            }
            else
            {
                Debug.Log("Не хватает ресурсов!");
            }
        }
    }

    void Explode()
    {
        Debug.Log("НАЧИНАЕМ ВЗРЫВ!");
        exploded = true;

        if (explosionPrefab != null && rocks != null)
        {
            // 1. Создаем взрыв
            GameObject explosionInstance = Instantiate(explosionPrefab, rocks.transform.position, Quaternion.identity);
            ParticleSystem ps = explosionInstance.GetComponent<ParticleSystem>();
            
            // 2. Запускаем разлет камней
            BreakRocks();

            // 3. Ждем окончания взрыва перед победой
            if (ps != null)
            {
                StartCoroutine(FinishEscapeAfterExplosion(ps.main.duration, explosionInstance));
            }
        }
    }

    void BreakRocks()
    {
        foreach (Transform rock in rocks.transform)
        {
            // УДАЛЯЕМ Mesh Collider (он несовместим с динамической физикой)
            MeshCollider meshCol = rock.GetComponent<MeshCollider>();
            if (meshCol != null)
            {
                Destroy(meshCol);
            }

            // Добавляем Rigidbody
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            if (rb == null) 
            {
                rb = rock.gameObject.AddComponent<Rigidbody>();
                rb.mass = 2f;
            }
            
            // Добавляем Box Collider вместо Mesh Collider
            if (rock.GetComponent<Collider>() == null)
            {
                rock.gameObject.AddComponent<BoxCollider>();
            }

            // Отцепляем от родителя
            rock.SetParent(null);
            
            // Включаем гравитацию
            rb.useGravity = true;
            
            // Толкаем от центра взрыва
            Vector3 direction = (rock.position - rocks.transform.position).normalized;
            direction += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.3f, 0.7f), Random.Range(-0.5f, 0.5f));
            
            rb.AddForce(direction * 2000f);
        }

        Invoke("HideRocksContainer", 0.5f);
    }

    void HideRocksContainer()
    {
        rocks.SetActive(false);
    }
       IEnumerator FinishEscapeAfterExplosion(float delay, GameObject explosion)
    {
        // ждём, пока взрыв проиграется
        yield return new WaitForSeconds(delay);
        
        // теперь убираем камни
        if (rocks != null)
        {
            rocks.SetActive(false);
        }

        // уничтожаем объект взрыва (чтобы не висел в памяти)
        if (explosion != null)
        {
            Destroy(explosion);
        }
        if (finishTrigger != null)
            finishTrigger.EnableFinishLine();

        Debug.Log("YOU ESCAPED!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}