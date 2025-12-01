# 배고픈 개구리 (Hungry Frog)

---

## 프로젝트 개요

**배고픈 개구리**는 모바일 환경을 대상으로 한 3D 캐주얼 게임으로,  
아래 네 가지를 주요 목표로 개발되었습니다.

1. Git을 이용한 버전 관리
2. CI/CD 파이프라인 구축
3. PlayFab을 이용한 플레이어 데이터 수집 및 관리
4. Google Play Game Services(GPGS) 연동 및 Play Store 내부 테스트 트랙 배포 자동화

- [게임 플레이 영상](https://youtu.be/8OX1WxW7JWU)
- [프로젝트 기술서](https://drive.google.com/file/d/1j_0mr2SqTEe4ZgoKtfy9mRc__UQpowkN/view?usp=drive_link)

---

## 기술 스택

- **개발 언어**
  - C#

- **엔진 / 개발 도구**
  - Unity
  - Visual Studio
  - Visual Studio Code

- **백엔드 / 플랫폼**
  - Google Play Game Services (GPGS)
  - PlayFab

- **CI/CD**
  - GitHub Actions (Self-Hosted Runner)

---

## 주요 기술

### 1) Google Play Game Services (GPGS)

- **자동 로그인 및 계정 연동**
  - 사용자가 Google 계정으로 자동 로그인하여, 별도의 회원가입 과정 없이 플레이어를 식별할 수 있도록 구현했습니다.
- **업적(Achievement) 시스템**
  - Google Play Console에 등록한 업적과 연동하여, 게임 내 조건을 달성하면 업적 해금 및 알림 팝업이 표시됩니다.
- **ID 중복 방지 및 PlayFab 연동**
  - Google 계정에서 가져온 ID에 랜덤 태그를 덧붙여 중복을 방지하고,
    해당 ID를 기반으로 PlayFab 계정을 생성·연동하는 흐름을 구성했습니다.

<details>
  <summary>GPGS 로그인 플로우 요약 보기</summary>

  1. GPGS 자동 로그인 시도  
  2. **로컬에 저장된 ID 존재 여부 확인**
     - 있음 → 저장된 ID로 로그인 시도  
     - 없음 → Google 계정에서 ID를 받아와 새 ID 생성
  3. PlayFab 계정 생성 및 연동
  4. ID 중복 방지를 위해 랜덤 태그를 자동 부여
</details>


### 2) CI/CD 파이프라인 (GitHub Actions + Self-Hosted Runner)

- **자동 빌드 & 테스트를 통한 빠른 피드백**
  - GitHub Actions 워크플로우 조건에 따라 빌드/테스트를 자동 수행하여,
    오류를 초기에 발견하고 빠르게 수정할 수 있도록 구성했습니다.
- **Self-Hosted Runner 도입**
  - GitHub Hosted 러너의 용량·제한 문제를 해소하기 위해,
    Ubuntu 기반 Self-Hosted Runner를 구성하여 Unity 빌드 및 배포를 수행했습니다.
- **Play Store 자동 배포**
  - Game-CI 및 GitHub Actions를 활용해, 태그/브랜치 기준으로 빌드된 결과물을
    Google Play Console 내부 테스트 트랙에 자동 업로드하는 파이프라인을 구축했습니다.

<details>
  <summary>워크플로우 구성 요소 예시</summary>

  - 리포지토리 체크아웃
  - Unity 프로젝트 빌드 (Android)
  - 빌드 아티팩트 업로드
  - Google Play 서비스 계정 키를 사용한 인증
  - Play Store 내부 테스트 트랙 자동 배포
</details>


### 3) 데이터 관리 (PlayFab + 로컬 JSON)

- **클라우드 기반 점수/데이터 관리**
  - 플레이어의 점수를 PlayFab 서버에 저장하여 관리하며,
    앱 삭제나 기기 교체 시에도 데이터가 유지되도록 했습니다.
- **리더보드 구현**
  - PlayFab의 통계를 이용해 플레이어 점수를 집계하고,
    게임 내 리더보드 UI로 상위 랭커 목록을 제공하도록 구현했습니다.
- **Custom ID 변경 및 로컬 저장**
  - 플레이어가 리더보드에 표시되는 Custom ID를 변경할 수 있는 기능을 제공하며,
    중복 방지를 위해 랜덤 태그를 자동으로 부여합니다.
  - 사용자의 Custom ID를 JSON 파일 형태로 로컬에 저장/불러오기 하여,
    재접속 시에도 동일한 ID를 사용할 수 있습니다.

<details>
  <summary>데이터 처리 흐름 요약 보기</summary>

  1. 게임 종료/스코어 갱신 시 → PlayFab에 점수 저장  
  2. 리더보드 화면 진입 시 → PlayFab 리더보드 데이터 조회  
  3. Custom ID 변경 시 →  
     - 새 ID + 랜덤 태그 생성  
     - PlayFab에 업데이트  
     - 로컬 JSON 파일에 함께 저장
</details>


### 4) 최적화 (ScriptableObject, Object Pool, Singleton)

- **ScriptableObject 기반 데이터 관리**
  - 아이템·오브젝트 데이터를 ScriptableObject로 관리하여,
    런타임에 객체 사본을 불필요하게 생성하지 않고 원본을 참조하도록 설계했습니다.
- **Object Pool**
  - 자주 생성/파괴되는 오브젝트를 미리 생성해 두고 풀에서 재사용하여,
    GC 부담과 Instantiate/Destroy 호출을 줄이고 성능을 최적화했습니다.
- **Singleton + JSON Utility**
  - 전역 매니저(Singleton)를 통해 중복 생성을 방지하고,
    일관된 상태를 유지하면서 설정 값을 공유하도록 구성했습니다.
  - Unity의 `JsonUtility`를 사용해 사용자 설정값(마스터/BGM/SFX 볼륨, 표시 이름 등)을
    JSON 형식으로 저장·로드하는 구조를 가지고 있습니다.

<details>
  <summary>예: PlayerData에 저장하는 설정 값</summary>

  - 마스터 볼륨
  - BGM 볼륨
  - SFX 볼륨
  - 리더보드에 표시되는 Display ID
</details>

---

## 테스트 및 자동화

- **내부 테스트**
  - GitHub Actions 파이프라인을 통해 자동 빌드된 APK를
    Play Store 내부 테스트 트랙에 배포하여 테스트를 진행했습니다.
- **빌드·배포 자동화**
  - 태그/브랜치 기준으로 워크플로우를 트리거하여,
    빌드와 배포 과정을 자동화함으로써 반복 작업을 최소화했습니다.

