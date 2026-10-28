# 유니티 개발을 하면서 자주 사용하는 코드들을 모아놓았습니다
> ## ChipmunkUtility 유니티 패키지 매니저 추가하기
> ``` https://github.com/Chipmunk-Rex/Chipmunk-Library.git?path=/Utility``` <br>

## Chipmunk Modules 설치

Modules는 Utility에 의존합니다. Git으로 설치할 때는 Unity Package Manager에서 Utility를 먼저 추가한 다음 Modules를 추가하거나, 사용하는 프로젝트의 `Packages/manifest.json`의 `dependencies`에 두 항목을 함께 등록하세요.

```json
"com.devlab.chipmunklibrary.utility": "https://github.com/Chipmunk-Rex/Chipmunk-Library.git?path=/Utility",
"com.devlab.chipmunklibrary.modules": "https://github.com/Chipmunk-Rex/Chipmunk-Library.git?path=/Modules"
```

패키지의 `package.json`에는 의존 버전을 선언하고, 실제 Git 설치 경로는 사용하는 프로젝트에서 관리합니다.

## 기능 설명
> ### Enum Extensions
> > GetNext<T>(): Retrieves the next value in an enum, wrapping around to the first value if at the end. <br>
> > GetPrev<T>(): Retrieves the previous value in an enum, wrapping around to the last value if at the beginning. <br>
> > GetRandom<T>(): Returns a random value from an enum. <br>
> > IsFirst<T>() / IsLast<T>(): Checks if the current value is the first or last in the enum. <br>
