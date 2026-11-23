using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrato padrão para objetos decorativos que se movem e possuem limite de desativação.
/// </summary>
public interface IMovable {
    void SetSpeed(float value);
    void SetDirection(Vector2 value);
    void Movement(bool isMovement);
    void SetLimit(float value);
}

/// <summary>
/// Controla o deslocamento contínuo de um elemento decorativo e, opcionalmente,
/// sua rotação no eixo Z (usado para folhas girando enquanto se movem).
/// </summary>
public class Decor : MonoBehaviour, IMovable {
    [SerializeField] private bool rotateLeaf = false;      // Ativa o giro contínuo (marcar no prefab de folha)
    [SerializeField] private float rotationSpeed = 120f;   // Velocidade de rotação em graus por segundo

    private float speed;
    private Vector2 direction;
    private float limit;
    private bool isFreeForMove = false;

    /// <summary>
    /// Atualiza a posição e a rotação do objeto a cada frame quando o movimento está ativo.
    /// </summary>
    void Update() {
        if (isFreeForMove) {
            // Deslocamento independente da taxa de quadros (Time.deltaTime)
            transform.position = (Vector2)transform.position + speed * direction * Time.deltaTime;

            // Em jogos 2D, a rotação visível no plano da tela ocorre no eixo Z
            if (rotateLeaf) {
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            }

            // Desativa o objeto ao cruzar o limite esquerdo para retornar à pool
            if (transform.position.x <= limit) gameObject.SetActive(false);
        }
    }

    public void Movement(bool isMovement) {
        isFreeForMove = isMovement;
    }

    public void SetDirection(Vector2 value) {
        direction = value;
    }

    public void SetSpeed(float value) {
        speed = value;
    }

    public void SetLimit(float value) {
        limit = value;
    }
}