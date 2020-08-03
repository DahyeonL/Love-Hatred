# 게임 개요

  ![image](https://user-images.githubusercontent.com/12894014/89166365-e6216800-d5b4-11ea-8065-9d16d6886538.png)

# 게임의 특징

- <strong>테라포밍</strong>
  -
  - 모바일 게임인데다 AR게임인 점을 고려했을 때, 좀 더 원활한 게임 진행을 위해서 일꾼의 역할에서 자원 수집 제거
  - 대신 건물에서 자원을 매 초 자동으로 수집하고, 왼쪽 사진처럼 영토를 확장해 나가면서 맵 곳곳에 있는 성소를 획득하면 자원 수집량이 증가하는 방식으로 개발
  - 성소의 획득 여부는 Unity의 overlapcapsule을 사용하여 성소 근처에 사용자의 건물이 있는지 확인하여 소유권을 판단하는 방식으로 구현
  ![image](https://user-images.githubusercontent.com/12894014/89166447-081aea80-d5b5-11ea-93f0-4f8e43b9c580.png)
 
- <strong>전투</strong>
  -
  - 유닛의 공격은 overlapCapsule을 사용하여 구현
  - 일정 범위 안의 적을 따라가고, 사거리 안의 적을 공격할 수 있게끔 구현
  - 공격 대상의 주변에도 피해를 입히는 splash 공격또한 overlapCapsule을 사용하여 구현
  - 첨부된 코드는 근거리 공격을 하는 유닛의 코드 일부분
  - 원거리 공격도 공중 유닛을 때릴 수 있다는 점을 빼면 동일한 방식으로 구현
  - 주변 유닛을 회복시키거나 공격력을 올려주는 버프 유닛 구현
  - ![image](https://user-images.githubusercontent.com/12894014/89166474-10732580-d5b5-11ea-9e50-7c0cffedbfd5.png) 
  
- <strong>AR Move</strong>
  -
  - 사진에 하늘색 블록이 AR Move 블록
  - 사용자는 핸드폰을 들고 맵 안과 밖을 돌아다니며 시점을 이동할 수 있는데 위에있는 사진처럼 2층 블록 두개와 AR Move 블록이 연결된 것 처럼 보일 때 밑의 사진처럼 두 2층 블록을 지상 유닛이 이동 할 수 있게 됨 (AR Move)
  - 카메라에서 블록 방향으로 Raycast를 쏴서 세 개의 블록이 붙어있는 것 처럼 보이는지 판단하는 함수를 구현 (상)
  - 유닛의 이동은 기본적으로 Navmesh를 사용하였는데, 유닛의 목적지를 2층 블록의 모서리로 바꿔주고, 모서리 근처로 가면 유닛을 연결된 다른 블록으로 옮기는 방식으로 구현 (하)
  - ![image](https://user-images.githubusercontent.com/12894014/89166491-15d07000-d5b5-11ea-8504-2f35d457510f.png)
  - ![image](https://user-images.githubusercontent.com/12894014/89166486-136e1600-d5b5-11ea-8486-dec8bdd95edf.png)
