# ProjectNK
---
### 전체 시스템 설계
#### 전체 시스템 볼륨
<img width="1073" alt="볼륨 설계" src="https://github.com/user-attachments/assets/b96ed11e-d407-471b-9560-bc1b4f384a79" />

#### 메인 프로그램 컨셉
전투 씬에서 필요한 로직들(카메라, 전투방식, 승리조건) 모듈화 하여 <br> 한개의 메인 플레이 컨트롤러에서 모듈 데이터 변경으로 여러가지 방식을 플레이 할 수 있도록 </br> **모듈** 컨셉으로 프로그램을 구상 </br>
<img width="951" alt="image" src="https://github.com/user-attachments/assets/2e7c62f8-214e-4890-8cc2-9a2994930acd" />


### 메인 시스템
#### Character Data Control flow
<img width="698" alt="image" src="https://github.com/user-attachments/assets/db97628f-4553-4ccf-9a4a-007c98d4d6e4" />

### 네트워크 연동

### UI

#### Mvp 패턴 기반 UI 설계
<img width="691" alt="image" src="https://github.com/user-attachments/assets/29a8b1dd-21ab-4d2d-a879-1b862f878698" />


```c#
/// model_UI.cs
public abstract class Model_UI : IModel_UI {
    public Model_UI() {}
}
/// presenter_UI.cs
public abstract class Presenter_UI<Model, View> : IPresenter_UI where Model : IModel_UI, new() where View : IView_UI
{
    protected Model _model;
    protected View _view;
    public IPresenter_UI Init(IView_UI view) {
        _model = new Model();
        _view = (View)view;
        return this;
    }
}
/// view_UI.cs
public abstract class View_UI<Presenter, Model> : MonoBehaviour, IView_UI where Presenter : IPresenter_UI, new() where Model : IModel_UI
{
    protected Presenter _presenter;
    public View_UI() {
        CreatePresenter();
    }
    protected abstract void CreatePresenter();
    public virtual void UpdateUI() { }
}
```

### 테스트 코드




이 프로젝트는 특정 게임에서 영감을 얻어 만들어졌습니다.
