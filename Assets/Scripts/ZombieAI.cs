using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour
{
    [Header("Настройки")]
    public Transform[] patrolPoints;
    public float chaseDistance = 10f;
    public float loseDistance = 15f;
    public GameObject gameOverPanel;
    
    [Header("Зрение")]
    public Transform eyes; // Перетащи сюда пустой объект на уровне "глаз" зомби
    public LayerMask coverLayer; // Слой для укрытий (настрой в инспекторе)
    public float viewCheckInterval =3f; // Проверка зрения не каждый кадр (оптимизация)
    public float loseDelay = 2f;
    
    private NavMeshAgent agent;
    private Transform player;
    private int currentPoint = 0;
    private bool isChasing = false;
    private float lastViewCheck;
    private bool canSeePlayer; // Кэшируем результат проверки
    private float lastTimeSeen;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Если глаза не назначены — создаём автоматически
        if (eyes == null)
        {
            eyes = new GameObject("Eyes").transform;
            eyes.parent = transform;
            eyes.localPosition = new Vector3(0, 1.6f, 0); // Уровень головы
        }
        
        gameOverPanel.SetActive(false);
        GoToNextPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Проверяем линию обзора не каждый кадр, а с интервалом (экономим ресурсы)
        if (Time.time - lastViewCheck > viewCheckInterval)
        {
            canSeePlayer = CheckLineOfSight(distance);
            lastViewCheck = Time.time;
        }

        // Логика переключения состояний
        if (canSeePlayer && distance < chaseDistance)
        {
            isChasing = true;
            lastTimeSeen = Time.time; // запоминаем, когда видели игрока
        }
        else if (!canSeePlayer)
        {
            // Игрок не виден: если прошло больше loseDelay секунд — теряем его
            if (Time.time - lastTimeSeen > loseDelay || distance > loseDistance)
            {
                isChasing = false;
            }
        }
        else if (distance > loseDistance)
        {
            // Игрок убежал слишком далеко
            isChasing = false;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Patrol();
        }
    }

    bool CheckLineOfSight(float distance)
    {
        if (distance > chaseDistance) return false;

        Vector3 direction = (player.position - eyes.position).normalized;
        
        // === ОТЛАДКА: видим луч в сцене (красная линия) ===
        Debug.DrawRay(eyes.position, direction * chaseDistance, Color.red, 2f);
        
        // Пускаем луч БЕЗ маски слоя — проверяем ВСЁ
        if (Physics.Raycast(eyes.position, direction, out RaycastHit hit, chaseDistance))
        {
            // === ОТЛАДКА: смотрим в Console, во что попал луч ===
            Debug.Log($"Ray hit: {hit.collider.name} | Layer: {hit.collider.gameObject.layer} | Distance: {hit.distance:F2}");
            
            // 1. Если луч попал в САМОГО игрока — значит, видим его (препятствий не было)
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            
            // 2. Если луч попал во что-то ДРУГОЕ на пути (стена, машина, бак) — игрок скрыт
            // Проверяем, что хит ближе, чем игрок (на случай неточностей)
            if (hit.distance < distance * 0.95f) // 0.95 — небольшой запас от погрешностей
            {
                return false; // Препятствие найдено, игрок не виден
            }
        }
        
        // Луч не попал ни во что до игрока — игрок на прямой видимости
        return true;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GAMEOVER");
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}