/** \page BWYou.Web.MVC BWYou.Web.MVC
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - ASP.NET MVC + Entity Framwork + Identity 공통 처리
 *  \section advenced 추가정보
 *          - Version : 0.3.2.8
 *          - Last Updated : 2016.12.09
 *              -# array로 생성, 삭제 기능
 *              -# 영문 메세지들로 여러부분 변경중.
 *          - Last Updated : 2016.11.24 Version : 0.3.2.7
 *              -# 로깅 강화
 *          - Updated : 2016.11.17 Version : 0.3.2.6
 *              -# GetWhereClause 옵션 다양화. nullable 처리 시 Equal 버그 수정. 테스트 강화
 *              -# FilterableAttribute, UpdatableAttribute 조건 강화
 *          - Updated : 2016.11.07 Version : 0.3.2.5
 *              -# MessageHandler에서 Custom 코드 사용 때 response.ReasonPhrase null 처리 버그 수정
 *          - Updated : 2016.11.04 Version : 0.3.2.4
 *              -# 확장 함수에서 null 처리 버그 수정
 *              -# mvc 용의 ModelStateDictionary를 http wep api용 ModelStateDictionary로 사용 하기 위한 확장 함수 추가
 *          - Updated : 2016.11.01 Version : 0.3.2.3
 *              -# ErrorResultViewModel 에서 dynamic Etc 변수 추가
 *          - Updated : 2016.10.31 Version : 0.3.2.2
 *              -# VM 처리를 위한 컨트롤러 추가
 *          - Updated : 2016.10.27 Version : 0.3.2.1
 *              -# NotImpl 버그 수정, Update 시 재확인 할 때 발생하는 버그 수정
 *          - Updated : 2016.10.27 Version : 0.3.2.0
 *              -# IBWModel 을 IDbModel로 이름 변경
 *              -# IIdModel, ICUModel 인터페이스 추가 및 이를 구현 하는 형식으로 BWModel 수정
 *              -# BWModel을 Generic 처리 후 Id를 int?, long?, string 용으로 하위 모델들 만들고 그에 따른 서비스, 컨트롤러 추가 생성
 *          - Updated : 2016.10.27 Version : 0.3.1.0
 *              -# DB 접속 하는 곳은 동기 함수 모두 제거하여 비동기를 사용하도록 함.
 *              -# Id를 int가 아닌 다른 것도 가능하도록 generic 처리
 *          - Updated : 2016.10.26 Version : 0.3.0.1
 *              -# 관리 포인트로만 사용 할 IBWModel 추가
 *              -# 서비스에 있던 GetWhereClause 함수를 IBWModel의 확장함수로 처리.
 *          - Updated : 2016.09.12 Version : 0.3.0.0
 *              -# IEntityService의 비동기 함수들 및 Query 관련 함수들 추가
 *              -# BWApiController의 비동기 함수들 추가
 *              -# IModelLoader 인터페이스의 LoadModelAsync 추가
 *          - Updated : 2016.09.06 Version : 0.2.3.0
 *              -# IModelLoader 구조 변경
 *          - Updated : 2016.03.18 Version : 0.2.2.5
 *              -# Deleted 상태에서 UpdateDT 변경 시도 하는 버그 수정
 *          - Updated : 2016.03.17 Version : 0.2.2.4
 *              -# BWEntityService에서 GetWhereClause 자식에게 노출
 *          - Updated : 2016.02.29 Version : 0.2.2.3
 *              -# DbContextRepository에서 DBSet, DBContext 노출
 *          - Updated : 2015.11.16 Version : 0.2.2.2
 *              -# RequireMappedHttpsAttribute 추가
 *              -# 관리 되고 있는 Entity의 수동 업데이트 경고 처리
 *          - Updated : 2015.06.02 Version : 0.2.2.1
 *              -# 바인딩모델을 모델에 맵핑 하기 위한 MapFromBindingModelToBaseModel 확장 함수 추가
 *          - Updated : 2015.04.29 Version : 0.2.2.0
 *              -# 관계 동시 삭제를 위한 ActivateRelation4Cascade 확장 함수 추가
 *              -# 간단한 모델 맵핑을 위한 MapFrom 확장 함수 추가
 *          - Updated : 2015.04.20 Version : 0.2.0.0
 *              -# 최초 정리
 *          - 구현 사항
 *              -# 공통 Model, ViewModel, Dao, Service, Controller, Etc 정리
 *              -# 행 복사를 위한 Clone 확장
 *          - 미구현 사항
 *              -# BWModel을 class로 더 범용적으로 구현되도록 해야 함
 * 
 *  \section explain 설명
 *          - ASP.NET MVC 관련 클래스
 * 
 *  \section Transfer Transfer
 *          - ASP.NET MVC 관련 클래스
 * 
 *          - 사용예)
 *  \code
            



 *  \endcode

 * 
 */

namespace BWYou.Web.MVC
{

}
