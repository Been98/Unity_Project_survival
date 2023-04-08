using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetBookText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] txtBook; //0 =l_title, 1 = r_ti, 2 = l_main, 3 = r_main
    [SerializeField] private Button[] btnBook; //책 넘기기 버튼 0 = left, 1 = right;
    [SerializeField] private Button[] btn_link;
    private int page = 1;
    private int maxPage = 4; //마지막 페이지
    void Start()
    {
        btnBook[0].onClick.AddListener(setDownPage);
        btnBook[1].onClick.AddListener(setUpPage);
    }

    private void setUpPage()
    {
        if (page < maxPage)
            page++;
        ChangeTxt();
    }
    private void setDownPage()
    {
        if (page > 1)
            page--;
        ChangeTxt();
    }
    private void ChangeTxt()
    {
        switch (page)
        {
            case 1:
                for (int i = 0; i < btn_link.Length; i++)
                    btn_link[i].gameObject.SetActive(false);
                btnBook[0].gameObject.SetActive(false);
                txtBook[0].text = "무탈한 인생";
                txtBook[2].text = "이 책은 무인도 탈출 백과사전이다.\n\n\n 김용규 지음 \n 최영빈 옮김 \n 출판사 (주)정수현";
                txtBook[1].text = "3의 법칙";
                txtBook[3].text = "공기없이는 3분\n\n온도없이는 3시간\n\n물없이는 3일\n\n식량없이는 3주를 버틸 수 있답니다.";
                break;
            case 2:
                for (int i = 0; i < btn_link.Length; i++)
                    btn_link[i].gameObject.SetActive(true);
                btnBook[0].gameObject.SetActive(true);
                txtBook[0].text = "어디서 자야할지 모르겠나요 ?";
                txtBook[2].text = "코코넛 나무와 같이 떨어질 물건이 있는 곳 아래는 위험해요.\n\n바닷가 근처에서는 조수간만의 차를 잘 확인해야 해요.\n\n비바람을 피할 수 있다면 체온이 유지할 수 있어요.\n\n주변이 막혀 시야가 가려지지 않으면 구조 요청하기가 쉬워요.\n\n물이 흐르는 장소 근처에는 동물과 음식이 많아요.";
                txtBook[1].text = "따끈따끈 불 피우기";
                txtBook[3].text = "건전지와 은박지or철수세미 --> 링크\n물을 담을 것을 이용한 방법 --> 링크\n보우드릴-- > 링크\n나무와 나무의 마찰-- > 링크";
                break;
            case 3:
                for (int i = 0; i < btn_link.Length; i++)
                    btn_link[i].gameObject.SetActive(false);
                btnBook[1].gameObject.SetActive(true);
                txtBook[0].text = "아무거나 마시면 안돼요 !";
                txtBook[2].text = "바닷물을 마시면 삼투현상으로 더 많은 물이 체외로 방출돼요.\n\n건강하지 않은 조난 상태의 오줌은 나튜륨 과다로 바닷물과 같아요.\n\n흐르는 개울이나 강은 박테리아가 있을 수 있어 끓여 마셔야 해요.\n\n비가 올 때 천막이나 바가지를 빗물로 씻은 후 물을 모을 수 있어요.\n\n아침이슬을 모아도 돼요.";
                txtBook[1].text = "배가 고파요ㅠ";
                txtBook[3].text = "배가 고파도 물이 충분하지 않으면 음식을 먹으면 안돼요ㅠ\n음식을 소화하느라 체내 수분을 사용하거든요.\n\n무슨 과일인지 모르겠다면 팔에 문질러봐요!\n이 때 발진이 올라온다면 먹으면 안돼요. 괜찮더라도 입술에도 문질러보기!\n\n음식은 최대한 기생충이 있을 수 있으니 익혀서 먹는게 좋아요.\n\n작살질은 낮에는 승산이 전혀 없으니 낚시를 해보는게 좋아요.\n\n밤에 바다에는 야행성인 상어의 위험이 있으니 항상 조심!\n\n물고기의 내장은 독성이 있을 수있으니 웬만하면 버리는게 좋아요!\n\n함정을 설치한다면 복잡한 매커니즘의 함정보다는 간단한 항아리 모양의 구덩이가 좋아요.\n\n";
                break;
            case 4:
                btnBook[1].gameObject.SetActive(false);
                txtBook[0].text = "도와주세요!!";
                txtBook[2].text = "가장 좋은 방법은 불 지피기예요. 연기를 삼각형 모양으로 피우면 국제적인 조난신호랍니다.\n\n불이 없다면 큰 돌이나 모래를 이용해 sos를 써보세요. \n다만 이 방법은 인접 비행중인 수색 항공기에 대해서만 효과가 있답니다ㅠ\n\n만약 손을 들어 표시할 때는 한손만 들지말고 두손을 v자로 들기!\n한손만 들면 괜찮다는 수신호랍니다.";
                txtBook[1].text = "이런상황 위험해요 !";
                txtBook[3].text = "해파리가 해변가로 모여들면 폭풍이 몰아칠 징조니 피하세요!\n\n해파리에 쏘였을 때는 물이나 식초말고 바닷물을 10분정도 뿌려 씻어내세요.\n물이나 식초는 해파리의 자포세포를 터뜨려 독이 더 분출된답니다.\n\n포식 동물들은 굴에 많이 살고 있답니다.\n\n벌에 쏘였을 때는 핀셋이나 집개 말고 카드와 같은 물건으로 밀어서 뽑아야해요!\n\n뱀에게 물렸다면 입으로 빨아서 독을 빼내지 마세요! 감염 및 중독의 위험이 있답니다.";
                break;
        }
    }
    public void goLink(int num)
    {
        switch (num)
        {
            case 1:
                Application.OpenURL("https://youtu.be/nvAV-IroeMM");
                break;
            case 2:
                Application.OpenURL("https://youtu.be/v1FcWABNh2w");
                break;
            case 3:
                Application.OpenURL("https://youtu.be/S4hRZVxZaI0");
                break;
            case 4:
                Application.OpenURL("https://youtu.be/zumDlCpE_Rk");
                break;
        }
    }
}