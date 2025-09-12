using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;

    private Vector2 inputDir;
    public float speed;

    public SpriteRenderer sr;
    private Rigidbody2D rb;

    private Vector2 _velocity;
    [HideInInspector]public GameObject curUI;
    public GameObject UIChanger;

    public GameobjectEventSO gameobjectEventSO;

    [SerializeField] private GameObject bagUI;

    public TransformEventSo GetPlayerTransformEventSO;
    public VoidEventSO TryGetPlayerTransEventSO;

    float playMinute;//分钟
    public bool canMove;

    public TileData curTileData;
    public Vector3Int curTilePos;
    public HandSelectManager handSelect;
    public SlotUI curSlot;
    public SlotUI selectedSlot;
    public TileSelector selectedTile;
    public GameobjectEventSO TileSelectorEventSO;
    public bool canUse;
    public bool keepUse;
    public float useDuration;//使用间隔
    private float useTimeCnt;
    public bool canInteract;

    private void Awake()
    {
        canMove = true;
        inputControl = new PlayerInputControl();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playMinute = 0;
        sr.allowOcclusionWhenDynamic = false;
        Vector3 _pos = new Vector3(GeneralDataLoader.instance.database.database.x,GeneralDataLoader.instance.database.database.y,GeneralDataLoader.instance.database.database.z);
        transform.position = _pos;
    }
    private void OnEnable()
    {
        TileSelectorEventSO.OnEventRaised += SetSelectedTile;
        TryGetPlayerTransEventSO.onEventRaised += GiveTrans;
        gameobjectEventSO.OnEventRaised += ChangeUI;
        inputControl.Gameplay.OpenBag.started += OpenBag;
        inputControl.UI.CloseUI.started += CloseUI;
        inputControl.Gameplay.UseItem.started += OnUseItem;
        inputControl.Gameplay.UseItem.canceled += OnUseItem;
        inputControl.Gameplay.Interact.started += OnInteractItem;
        inputControl.Enable();
    }
    private void OnDisable()
    {
        TileSelectorEventSO.OnEventRaised -= SetSelectedTile;
        TryGetPlayerTransEventSO.onEventRaised -= GiveTrans;
        gameobjectEventSO.OnEventRaised -= ChangeUI;
        inputControl.Gameplay.OpenBag.started -= OpenBag;
        inputControl.UI.CloseUI.started -= CloseUI;
        inputControl.Gameplay.UseItem.started -= OnUseItem;
        inputControl.Gameplay.UseItem.canceled -= OnUseItem;
        inputControl.Gameplay.Interact.started -= OnInteractItem;
        inputControl.Disable();
    }
    private void Update()
    {
        selectedSlot = handSelect.selectedSlot?.GetComponent<SlotUI>();
        inputDir = inputControl.Gameplay.Move.ReadValue<Vector2>();
        playMinute += Time.deltaTime;
        useTimeCnt += Time.deltaTime;
        if (playMinute >= 60)
        {
            GeneralDataLoader.instance.database.database.playTime++;
            playMinute -=60f;
        }
        if(keepUse&&canUse&&curSlot!=null&&useTimeCnt>=useDuration)
        {
            useTimeCnt = 0;
            UseItem();
        }
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void SetSelectedTile(GameObject tileSelector)
    {
        selectedTile = tileSelector.GetComponent<TileSelector>();
    }
    private void Move()
    {
        _velocity.x = inputDir.x * speed;
        _velocity.y = inputDir.y * speed;
        rb.velocity = _velocity;
        if (inputDir.x > 0)
        {
            sr.flipX = false;
        }
        else if (inputDir.x < 0)
        {
            sr.flipX = true;
        }
    }

    private void ChangeUI(GameObject obj)
    {
        if (obj == curUI) return;
        if(curUI!=null)
        curUI.SetActive(false);
        curUI = obj;
        curUI.SetActive(true);
    }
    public void OpenBag(InputAction.CallbackContext context)
    {
        bagUI.GetComponent<BagUI>()?.Open();
        UIChanger.SetActive(true);
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        bagUI.GetComponent<BagUI>().CloseExtra();
        if(curUI!=null)
            curUI.SetActive(false);
        UIChanger.SetActive(false);
        curUI = null;
        Time.timeScale = 1f;
    }
    private void GiveTrans()
    {
        GetPlayerTransformEventSO.RaiseEvent(transform);
    }

    private void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.started && canUse && useTimeCnt>=useDuration)
        {
            useTimeCnt = 0;
            canInteract = false;
            UseItem();
            keepUse = true;
        }
        else if (context.canceled)
        {
            keepUse = false;
            canInteract = true;
        }
    }
    private void OnInteractItem(InputAction.CallbackContext context)
    {
        if(canInteract)
        {
            InteractItem();
        }

    }

    private void UseItem()
    {
        curSlot = selectedSlot;
        curTileData = selectedTile.SelectedTileData;
        if (curTileData != null)
        {
            curTilePos = selectedTile.SelectedPos;
        }
        if(curSlot != null&&curTileData!=null)
        ItemUseManager.UseAnimate(this,curSlot);
    }
    public void OnUseItemAnimationEvent()
    {
        ItemUseManager.UseItem(this, curSlot, curTileData,curTilePos);
    }

    private void InteractItem()
    {
        curSlot = selectedSlot;
        curTileData = selectedTile.SelectedTileData;
        if (curTileData != null)
        {
            curTilePos = selectedTile.SelectedPos;
        }
        if(curTileData != null)
        {
            ItemUseManager.Interact(this,curSlot,curTileData,curTilePos);
        }
    }
}
