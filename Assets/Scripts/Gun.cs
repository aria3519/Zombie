using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력
    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄약
    public int magCapacity = 25; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약


    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점


    private void Awake() {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; // 사용할 점을 두개로 변경 
        bulletLineRenderer.enabled = false; // 라인 렌더러를 비활성화 

    }

    private void OnEnable()
    {
        // 총 상태 초기화

        magAmmo = magCapacity; // 현재 탄창 가득 채우기 
        state = State.Ready; // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경 
        lastFireTime = 0; // 마지막으로 총을 쏜 시점 초기화
        // 총알 사이 사이 딜레이 타임을 주기 위해 
    }

    // 발사 시도
    public void Fire() 
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }

    }

    // 실제 발사 처리
    private void Shot() 
    {
        // use Raycast -> 레이저를 쏘는 방식 
        RaycastHit hit;
        // raycast에 의한 충돌 정보를 저장 하는 컨테이너
        Vector3 hitPosition = Vector3.zero;
        // 탄알이 맞은 곳을 저장할 변수

        // Raycast(시작 지점,방향,충돌 정보 컨테이너, 사정 거리)
        if(Physics.Raycast(fireTransform.position,fireTransform.forward,out hit,
            fireDistance))
        {
            // Ray가 어떤 물체와 충돌한 경우
            // 충돌한 상대방으로 부터 IDamgeable Component를 가져온다.
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // 상대방으로 부터 IDamageable Component를 가져오는데 성공 했다면?
            if(target != null)
            {
                // 상대방의 OnDamage 함수를 실행시켜 상대방에 Damage 주기
                target.OnDamage(damage, hit.point, hit.normal);
            }

            // Ray 가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // Ray가 다른물체와 충돌하지 않았을때
            // 탄알이 최대 사정거리까지 날아갔을때 위치를 충돌 위치로 사용 
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }


        // 발사 이펙트 재생 시작 
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;

        if(magAmmo<=0)
        {
            // 탄창에 탄이 없는 경우 empty로 상태 변환 
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition) 
    {
        muzzleFlashEffect.Play(); // 총구 화연 효과 재생 
        shellEjectEffect.Play(); // 탄피 배출 효과 재생

        gunAudioPlayer.PlayOneShot(shotClip); // 총격 소리 재생 
        // 선의 시작은 총구의 위치 
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);


        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() 
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo > magCapacity)
        {
            // 이미 재장전 중이거나 남은 탄알이 없거나
            // 탄창에 탄창이 이미 가득 찬 경우 재장전 못하게 바로 리턴됨
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        // 재장전 소리 재생
        gunAudioPlayer.PlayOneShot(reloadClip);
        
        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);
        
        // 탄창에 채울 탄알을 계산
        int ammoToFill = magCapacity - magAmmo;
        // 탄창에 채워야할 탄알이 남은 탄알 보다 많다면
        // 채워야할 탄알 수를 남은 탄알 수에 맞춰 줄임
        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        // 탄창을 채움
        magAmmo += ammoToFill;

        // 남은 탄알에서 탄창에 채운 만틈 탄알을 뺌
        ammoRemain -= ammoToFill;
        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}