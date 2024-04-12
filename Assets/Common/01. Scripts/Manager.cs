using UnityEngine;
using Jc;
public static class Manager
{
    // JC - Navigation : ���Ӹ�, �÷��̾ ��ġ�� Ÿ�� ������ (���� ��ã�� �뵵�� Ȱ��)
    public static NavigationManager Navi { get { return NavigationManager.Instance; } }
    // JC - Layer : ���� ��Ȳ�� �ʿ��� ���̾���� ��Ƴ��� LayerMask ������
    public static LayerManager Layer { get { return LayerManager.Instance; } }
    // JC - Data : ���� ������ ������
    public static DataManager Data {  get { return DataManager.Instance;} }
    // JC - Pool : ������Ʈ Ǯ��
    public static PoolManager Pool { get { return PoolManager.Instance;} }
    // JC - Scene : �� ��ȯ �Ŵ��� (�񵿱� �� �ε�)
    public static SceneManager Scene { get { return SceneManager.Instance; } }
    // JM - 
    public static ItemManager Item { get { return ItemManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // �̱��� ��ü����
        PoolManager.ReleaseInstance();
        NavigationManager.ReleaseInstance();
        LayerManager.ReleaseInstance();
        DataManager.ReleaseInstance();
        ItemManager.ReleaseInstance();
        SceneManager.ReleaseInstance();

        // �̱��� ��ü����
        PoolManager.CreateInstance();
        NavigationManager.CreateInstance();
        LayerManager.CreateInstance();
        DataManager.CreateInstance();
        ItemManager.CreateInstance();
        SceneManager.CreateInstance();
    }
}