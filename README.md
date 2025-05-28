# 배고픈 개구리

## 프로젝트 소개
Unity 엔진을 기반으로 제작한 3D 모바일 캐주얼 게임입니다.
﻿이 프로젝트는 Git을 통한 프로젝트 버전 관리, Git Action을 통한 CI/CD 파이프라인 구현, Playfab을 이용한 플레이어 데이터 관리, 플레이스토어 배포 및 GPGS 활용을 목표로 진행하였습니다.

## 프로젝트 개요
- 개발도구 : Unity, Visual Studio
- 개발언어 : C#
- CI/CD : Git Action(Self-Hosted)
- 백엔드 : Google Play Game Service, Playfab

## 주요 기능 :
**Google Play Game Service 연동**
  - 자동 로그인 : 게임 실행 시 Google 계정으로 자동 로그인 처리
  - 업적 & 도전 과제 : GPGS 업적 시스템과 게임 내 연동

**CI / CD (Git Action)**
  - 빌드 자동화 : Git tag 푸시 시 AAB 빌드 자동 실행
  - Self-Hosted Runner : Git Hosted의 빌드 용량 문제 해결을 위해 Self-Hosted 환경 구축
  - 플레이 스토어 배포 : 빌드 완료 후 플레이 스토어 자동 업로드

**Playfab 데이터 관리**
  - 리더보드 : PlayFab Leaderboard API를 활용한 점수 집계 및 표시
  - 사용자 식별 : GPGS 로그인 실패 시 Custom ID 부여 (중복 방지 태그 구현)
  - 데이터 저장 : 유저 Custom ID JSon 파일로 로컬 저장, 불러오기

**최적화 기법**
  - ScriptableObject : 자주 생성 및 파괴되는 오브젝트 데이터를 ScriptableObject로 관리하여 메모리 절약 및 수정, 추가 편의성 향상
  - Object Pool : 런타임 퍼포먼스 향상을 위해 오브젝트 풀에서 호출 및 반환을 통해 재사용

**데이터 관리**
  - 사운드, 리더보드 관리 오브젝트 등 핵심 클래스는 싱글톤 패턴으로 설계하여 단일 객체를 보장
  - JSON 저장 : 사용자의 설정(세팅 값, ID 등)을 JSON 파일로 저장, 로드하여 일관성 유지

## 기대효과
**신속한 배포**
  - 자동화된 빌드 및 배포 파이프라인으로 릴리스 사이클 단축

**데이터 관리**
  - PlayFab과 JSON 파일을 통한 안정적인 유저 데이터 저장·로드

**최적화 및 퍼포먼스 향상**
  - ScriptableObject와 Object Pool 적용으로 메모리 사용량 및 런타임 성능 개선
