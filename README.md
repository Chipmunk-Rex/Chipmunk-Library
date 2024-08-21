# 유니티 개발을 하면서 자주 사용하는 코드들을 모아놓았습니다


# 설명
> ## FSM
>> * EntityState<,> : 모든 FSM의 State는 이 클래스를 상속받아야함
>>                    제네릭으로 State를 정의한 Enum의 타입과 IFSMEntity를 구현한 클래스를 받아옴
>> * FSMStateMachine<,> : FSMState를 관리해줌
>>                        제네릭은 EntityState와 동일하게 받아옴
>> * IFSMEntity<,> : FSM을 사용하는 클래스는 무조건 구현해야하는 인터페이스
>>                   제네릭은 위와 동일
> ## Pool
>> * IPoolable : 객체를 Pool에 담기위해 필수로 구현해야하는 인터페이스
>> * PoolManager : 경기게임마이스터고 유니티 심화반 방식의 PoolManager
>> * PoolListSO : PoolManager에서 풀링할 PoolItem들을 보관하는 리스트
>> * PoolItem : PoolManager에서 풀링할 PoolItem
>> * LightPoolManager : PoolManager에서의 SO를 생성해야하는 단점을 보완함 
>>                      인스펙터창에서 프리팹을 링크하면 더 편하게 풀링 할 수 있음
> ## UI Toolkit
> #### UI Toolkit과 관련된 모든 컴포넌트는 AddComponentMenu에서 Chipmunk/Toolkit/ 경로에 있음
>> * Tk_Parent : UI_Toolkit과 관련된 코드들이 상속받는 추상 클래스로,
>>               Enable될 때 현재 오브젝트에 있는 Document를 자동으로 가져옴
>> * Tk_Element<> : elementName으로 element를 찾아 변수에 담아줌
>>                  제네릭으로 Element의 타입을 받아옴 
>>                  (사용 예제 : Tk_Element<Button> O, TK_Element<ScriptableObject> X)
>> * * Tk_Btn : Button을 클릭했을때 onBtnClick 이벤트를 발행 
>> * * Tk_Display : element의 Display를 끄거나 킬 수 있음
>> * TK_Document : Document가 보이는걸 설정할 수 있음
>>                 보이기 시작할때 onShow, 숨겨질 때 onHide 유니티 이벤트를 발행
>> * TK_Singler : 이 컴포넌트가 달린 게임 오브젝트의 자식에 있는
> ## Utility
>> * ChipmunkLibrary : 자주 사용하는 기능들을 함수로 구현해놓음
>> <details>
>> <summary> ChipmunkLibrary내부 함수들</summary>
>>      GetComponentWhenNull<> component 변수가 비어있을때만 getComponent를 해주는 함수
>> </details>