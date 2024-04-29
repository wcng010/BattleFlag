using C_Script.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace C_Script.UI
{
    public class ButtonFunction : MonoBehaviour
    {
        //执行移动指令，点击后菜单消失，开启移动模式
        public void CurrentEntityMove()
        {
            if (GameManager.Instance.CurrentEntity != null)
            {
                GameManager.Instance.EntityMove();
            }
        }

        //执行移动指令，点击后菜单消失，开启攻击
        public void CurrentEntityAttack()
        {
            if (GameManager.Instance.CurrentEntity != null)
            {
                GameManager.Instance.EntityAttack();
            }
        }

        public void CurrentEntityRecover()
        {
            if (GameManager.Instance.CurrentEntity != null)
            {
                GameManager.Instance.EntityRecover();
            }
        }

        public void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ApplicationQuit()
        {
            Application.Quit();
        }


        public void ReadyStateFinished()
        {
            EventCentreManager.Instance.Publish(MyEventType.ReadyFinished);
            gameObject.SetActive(false);
        }
    }
}
