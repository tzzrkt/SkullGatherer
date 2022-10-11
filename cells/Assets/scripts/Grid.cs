using UnityEngine;
using System.Collections.Generic;
public class Grid : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public GameObject[] buildPrefabs;
    public GameObject[] grassPrefabs;
    public GameObject[] bounds;
    public GameObject[] skulls;

    public Material terrainMaterial;
    public Material edgeMaterial;

    public float waterLevel = .4f;
    public float sandLevel = .6f;
    public float grassLevel = .8f;

    public float scale = .1f;

    public float treeNoiseScale = .05f;
    public float treeDensity = .5f;

    public float buildNoiseScale = .05f;
    public float buildDensity = .5f;

    public float grassNoiseScale = .05f;
    public float grassDensity = .5f;

    public float skullNoiseScale = .05f;
    public float skullDensity = .5f;

    public float boundNoiseScale = 0.5f;
    public float boundDensity = .5f;
    public float minRidge = .2f;
    public float maxRidge = .8f;

    public float treescale = 1f;
    public float buildscale = 1f;
    public float grassscale = 1f;
    public float skullscale = 1f;
    public float boundscale = 1f;

    public Color water;
    public Color sand;
    public Color grass;
    public Color grave;
    public Color tree;

    public int size = 100;
    public GameObject plane;

    Cell[,] grid;

    void Start()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                float noiseValue1 = Mathf.PerlinNoise(x * scale * 2 + xOffset, y * scale * 2 + yOffset);
                float noiseValue2 = Mathf.PerlinNoise(x * scale * 4 + xOffset, y * scale * 4 + yOffset);
                noiseMap[x, y] = noiseValue * .9f + noiseValue1 * .6f + noiseValue2 * .3f;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                bool isSand = noiseValue > waterLevel && noiseValue < grassLevel;
                bool isGrass = noiseValue > grassLevel;
                Cell cell = new Cell(isWater, isSand, isGrass, false, false, false);
                grid[x, y] = cell;
            }
        }

        // DrawTerrainMesh(grid);
        //DrawEdgeMesh(grid);
        GenerateGrass(grid);
        GenerateTrees(grid);
        GenerateBuilds(grid);
        GenerateSkulls(grid);
        GenerateBounds(grid);
        DrawTexture(grid);
    }


    void DrawTerrainMesh(Cell[,] grid)
    {

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        //MeshCollider meshCo = gameObject.AddComponent<MeshCollider>();
        //meshCo.sharedMesh = null;
        //meshCo.sharedMesh = mesh;
    }

    void DrawEdgeMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
    }

    void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * size + x] = water;
                else if (cell.isSand && !cell.hasTrees && !cell.hasBuild)
                    colorMap[y * size + x] = sand;
                else if (cell.isGrass && !cell.hasTrees && !cell.hasBuild)
                    colorMap[y * size + x] = grass;
                else if (cell.hasTrees)
                    colorMap[y * size + x] = tree;
                else if (cell.hasBuild)
                    colorMap[y * size + x] = grave;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;

        MeshRenderer meshRenderer2 = plane.GetComponent<MeshRenderer>();
        meshRenderer2.material = terrainMaterial;
        meshRenderer2.material.mainTexture = texture;
        meshRenderer2.material.SetFloat("_Glossiness", 0f);

        meshRenderer.enabled = false;
    }

    void GenerateTrees(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
                float noiseValue1 = Mathf.PerlinNoise(x * treeNoiseScale * 2 + xOffset, y * treeNoiseScale + yOffset * 2);
                float noiseValue2 = Mathf.PerlinNoise(x * treeNoiseScale + xOffset * 4, y * treeNoiseScale + yOffset * 4);

                noiseMap[x, y] = (noiseValue + noiseValue1 + noiseValue2) * .33f;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    float v = Random.Range(0f, treeDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                        GameObject tree = Instantiate(prefab, transform);
                        tree.transform.position = new Vector3(x, 0, y);
                        //tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                        tree.transform.localScale = Vector3.one * treescale;
                        cell.hasTrees = true;
                    }
                }
            }
        }
    }

    void GenerateBuilds(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * buildNoiseScale + xOffset, y * buildNoiseScale + yOffset);
                float noiseValue1 = Mathf.PerlinNoise(x * buildNoiseScale * 2 + xOffset, y * buildNoiseScale + yOffset * 2);
                float noiseValue2 = Mathf.PerlinNoise(x * buildNoiseScale + xOffset * 4, y * buildNoiseScale + yOffset * 4);

                noiseMap[x, y] = (noiseValue + noiseValue1 + noiseValue2) * .33f;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.hasTrees)
                {
                    float v = Random.Range(0f, buildDensity);
                    if (noiseMap[x, y] < v)
                    {
                        bool exclude = false;
                        GameObject prefab = buildPrefabs[Random.Range(0, buildPrefabs.Length)];
                        if (prefab.transform.tag == "exclude") { exclude = true; }
                        GameObject build = Instantiate(prefab, transform);
                        build.transform.position = new Vector3(x, 0, y);
                        //tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        build.transform.rotation = Quaternion.Euler(0, -90, 0); //Random.Range(0, 3) * 90, 0);
                        build.transform.localScale = Vector3.one * buildscale;
                        cell.hasBuild = true;
                        cell.excludeSk = exclude;
                    }
                }
            }
        }
    }

    void GenerateGrass(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * grassNoiseScale + xOffset, y * grassNoiseScale + yOffset);
                float noiseValue1 = Mathf.PerlinNoise(x * grassNoiseScale * 2 + xOffset, y * grassNoiseScale + yOffset * 2);
                float noiseValue2 = Mathf.PerlinNoise(x * grassNoiseScale + xOffset * 4, y * grassNoiseScale + yOffset * 4);

                noiseMap[x, y] = (noiseValue + noiseValue1 + noiseValue2) * .33f;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    float v = Random.Range(0f, grassDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
                        GameObject grass = Instantiate(prefab, transform);
                        grass.transform.position = new Vector3(x, 0, y);
                        //grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                        grass.transform.localScale = Vector3.one * grassscale;
                    }
                }
            }
        }
    }

    void GenerateSkulls(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * skullNoiseScale + xOffset, y * skullNoiseScale + yOffset);
                float noiseValue1 = Mathf.PerlinNoise(x * skullNoiseScale * 2 + xOffset, y * skullNoiseScale + yOffset * 2);
                float noiseValue2 = Mathf.PerlinNoise(x * skullNoiseScale + xOffset * 4, y * skullNoiseScale + yOffset * 4);

                noiseMap[x, y] = (noiseValue + noiseValue1 + noiseValue2) * .33f;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.hasTrees && !cell.isSand && !cell.excludeSk)
                {
                    float v = Random.Range(0f, skullDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab = skulls[Random.Range(0, skulls.Length)];
                        GameObject skl = Instantiate(prefab, transform);
                        skl.transform.position = new Vector3(x, 0.2f, y);
                        //grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        skl.transform.rotation = Quaternion.Euler(0, Random.Range(0, 2) * 90, 0);
                        skl.transform.localScale = Vector3.one * skullscale;
                    }
                }
            }
        }
    }

    void GenerateBounds(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * boundNoiseScale + xOffset, y * boundNoiseScale + yOffset);
                    noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    //float v = Random.Range(0f, boundDensity);
                    //if (noiseMap[x, y] < v)
                    if (noiseMap[x, y] > minRidge && noiseMap[x, y] < maxRidge)
                    {
                        GameObject prefab = bounds[Random.Range(0, bounds.Length)];
                        GameObject bound = Instantiate(prefab, transform);
                        bound.transform.position = new Vector3(x, 0, y);
                        //grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        bound.transform.rotation = Quaternion.Euler(0, Random.Range(0, 2) * 90, 0);
                        bound.transform.localScale = Vector3.one * boundscale;
                    }
                }
            }
        }
    }
}