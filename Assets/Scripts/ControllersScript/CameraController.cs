using UnityEngine;
using System.Collections;
/// <summary>
/// Отвечает за движение камеры по вертикали за игроком
/// </summary>
public class CameraController : BaseGameObject
{
    [SerializeField]
    GameObject Player;

    /// <summary>
    /// Запоминаем высоту относительно игрока. 
    /// Необходимо чтоб при изменении настроек в сцене не лезть в скрипт
    /// </summary>
    float distanceToPlayer;

    /// <summary>
    /// Запоминаем минимальную высоту камеры. 
    /// Необходимо чтоб камера не проваливалась под текстуры вслед за игроком(при падении)
    /// </summary>
    float minimum;

    private void Start() {
        distanceToPlayer = ObjectPosition.y - Player.transform.position.y;
        minimum = ObjectPosition.y;
    }

    /// <summary>
    /// Получаем новую позицию относительно игрока и плавно перемещаем камеру за ним
    /// </summary>
    private void FixedUpdate() {
        var newPositionY = Mathf.Clamp(Player.transform.position.y + distanceToPlayer, minimum, Mathf.Infinity);
        var newPosotion = new Vector3(ObjectPosition.x, newPositionY, ObjectPosition.z);
        ObjectPosition = Vector3.MoveTowards(ObjectPosition, newPosotion, .4f);
    }
}
