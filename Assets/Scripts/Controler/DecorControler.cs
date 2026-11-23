using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Agrupa as configurações de spawn, aparência e movimento de um tipo de decoração
/// (ex.: nuvens, folhas ou linhas de vento) para exibição no Inspector.
/// </summary>
[System.Serializable]
public class DecorConfig {
    public GameObject prefab; // Prefab base que será instanciado ou reaproveitado
    public Vector2 direction = Vector2.left; // Direção base de deslocamento no mundo

    [Header("Area Spawn")]
    public Vector2 areaDownLeft = Vector2.zero; // Canto inferior esquerdo (define também o limite X de desativação)
    public Vector2 areaUpRight = Vector2.zero;  // Canto superior direito (borda X inicial após o spawn inicial)

    [Header("Atributes Spawn")]
    public int minQuant = 1;           // Quantidade mínima na pool / lote inicial
    public int maxQuant = 2;           // Quantidade máxima de instâncias simultâneas na pool
    public float minTimeSpawn = 1f;    // Intervalo mínimo (em segundos) entre spawns no loop
    public float maxTimeSpawn = 1.5f;  // Intervalo máximo (em segundos) entre spawns no loop

    [Header("Atributes Prefab")]
    public float minSpeed = 1.0f;      // Velocidade mínima de movimento
    public float maxSpeed = 2.0f;      // Velocidade máxima de movimento
    public float minScale = 0.7f;      // Escala mínima aplicada ao objeto
    public float maxScale = 1.1f;      // Escala máxima aplicada ao objeto
    public byte minAlpha = 200;        // Transparência mínima (0 = invisível, 255 = opaco)
    public byte maxAlpha = 255;        // Transparência máxima (0 = invisível, 255 = opaco)

    // Lista interna de reaproveitamento (Object Pooling) para evitar Instantiate/Destroy constantes
    [HideInInspector] public List<GameObject> pool = new List<GameObject>();
}

/// <summary>
/// Gerencia o nascimento, o reaproveitamento e os parâmetros iniciais dos elementos
/// decorativos em movimento (nuvens, folhas e rastros de vento) acoplados ao mundo.
/// </summary>
public class DecorControler : MonoBehaviour {
    [Header("Configs")]
    [SerializeField] private DecorConfig cloudConfig = new DecorConfig(); // Configuração das nuvens (ex.: Layer Background, Order 1)
    [SerializeField] private DecorConfig leafConfig = new DecorConfig();  // Configuração das folhas (ex.: Layer Decor, movimento diagonal)
    [SerializeField] private DecorConfig windConfig = new DecorConfig();  // Configuração das linhas de vento (ex.: Layer Decor)

    /// <summary>
    /// Chamado pela Unity no início da execução para popular o cenário e iniciar os loops de spawn.
    /// </summary>
    void Start() {
        InitConfig(cloudConfig);
        InitConfig(leafConfig);
        InitConfig(windConfig);
    }

    /// <summary>
    /// Gera um lote inicial de objetos distribuídos pela área e inicia a corrotina de spawn contínuo.
    /// </summary>
    private void InitConfig(DecorConfig cfg) {
        if (cfg.prefab == null) return; // Evita erro caso o prefab ainda não tenha sido atribuído no Inspector

        int aux = Random.Range(cfg.minQuant, cfg.maxQuant + 1); // +1 pois o limite superior em Random.Range(int, int) é exclusivo
        for (int i = 0; i < aux; i++) {
            Vector2 pos = GetRandomPosition(cfg, isStart: true);
            SpawnFromConfig(cfg, pos);
        }

        StartCoroutine(LoopSpawn(cfg));
    }

    /// <summary>
    /// Loop contínuo que aguarda um tempo aleatório e spawna um novo elemento na borda direita.
    /// </summary>
    private IEnumerator LoopSpawn(DecorConfig cfg) {
        while (true) {
            float waitTime = Random.Range(cfg.minTimeSpawn, cfg.maxTimeSpawn);
            yield return new WaitForSeconds(waitTime); // Pausa a corrotina sem travar o frame do jogo

            Vector2 pos = GetRandomPosition(cfg, isStart: false);
            SpawnFromConfig(cfg, pos);
        }
    }

    /// <summary>
    /// Sorteia uma posição dentro da área (no início) ou na borda direita com altura Y aleatória (no loop).
    /// </summary>
    private Vector2 GetRandomPosition(DecorConfig cfg, bool isStart) {
        Vector2 pos = Vector2.zero;
        if (isStart) {
            // Distribui dentro de todo o retângulo definido por areaDownLeft e areaUpRight
            pos.x = Random.Range(cfg.areaDownLeft.x, cfg.areaUpRight.x);
            pos.y = Random.Range(cfg.areaDownLeft.y, cfg.areaUpRight.y);
        } else {
            // Nasce na borda direita (X máximo) e varia a altura (Y)
            pos.x = cfg.areaUpRight.x;
            pos.y = Random.Range(cfg.areaDownLeft.y, cfg.areaUpRight.y);
        }
        return pos;
    }

    /// <summary>
    /// Reutiliza um objeto inativo da pool ou instancia um novo, aplicando escala, alpha e movimento.
    /// </summary>
    private void SpawnFromConfig(DecorConfig cfg, Vector2 pos) {
        float speed = Random.Range(cfg.minSpeed, cfg.maxSpeed);
        float scale = Random.Range(cfg.minScale, cfg.maxScale);
        float alpha = Random.Range(cfg.minAlpha, cfg.maxAlpha + 1) / 255f; // Converte 0..255 para 0f..1f
        // Normaliza a direção para que diagonais não aumentem a velocidade; usa Vector2.left se zerada
        Vector2 dir = cfg.direction.sqrMagnitude > 0.0001f ? cfg.direction.normalized : Vector2.left;

        GameObject obj = GetDisabledPrefab(cfg.pool);
        if (obj == null && cfg.pool.Count < cfg.maxQuant) {
            // Cria uma nova instância filha do DecorControler (acoplada ao mundo, não à câmera)
            obj = Instantiate(cfg.prefab, pos, cfg.prefab.transform.rotation, this.transform);
            cfg.pool.Add(obj);
        } else if (obj != null) {
            // Reaproveita um objeto desativado previamente
            obj.transform.position = pos;
            obj.SetActive(true);
        } else {
            return; // Limite máximo de instâncias ativas atingido no momento
        }

        // Define a escala de forma absoluta (Vector3.one * scale) para não acumular reduções a cada reuso
        obj.transform.localScale = Vector3.one * scale;

        // Ajusta o canal alpha (transparência) mantendo a cor base do SpriteRenderer
        SpriteRenderer sprRen = obj.GetComponent<SpriteRenderer>();
        if (sprRen != null) {
            Color c = sprRen.color;
            c.a = alpha;
            sprRen.color = c;
        }

        // Configura os parâmetros de movimento via interface IMovable
        IMovable scr = obj.GetComponent<IMovable>();
        scr?.SetSpeed(speed);
        scr?.SetDirection(dir);
        scr?.Movement(true);
        scr?.SetLimit(cfg.areaDownLeft.x);
    }

    /// <summary>
    /// Busca na pool algum GameObject atualmente desativado para ser reutilizado.
    /// </summary>
    private GameObject GetDisabledPrefab(List<GameObject> pool) {
        foreach (GameObject obj in pool) {
            if (!obj.activeSelf) {
                return obj;
            }
        }
        return null;
    }
}