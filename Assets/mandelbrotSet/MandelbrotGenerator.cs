using UnityEngine;
using System.Collections;

public class MandelbrotGenerator : MonoBehaviour {

    // Texture set to apply for the grid.
    private Texture2D texture;
    // Resolution of the output picture.
    int width = 4800;
    int height = 4800;

    // Change this values lead to different zooming.
    float xmin = -0.23f;
    float xmax = 1f;
    float ymin = -0.1566f;
    float ymax = 0.198373f;

    // Maximal number of iterations.
    // Change will lead to outputs of different smoothness and detalization.
    int maxIterations = 130;

    /// <summary>
    /// Unity function to initialize the object.
    /// </summary>
    void Start()
    {
        texture = new Texture2D(height, width);
        // Created texture assigned to the renderer unity object.
        GetComponent<Renderer>().material.mainTexture = texture;

        Mandelbrot();
    }

    /// <summary>
    /// Method which calculates an iteration, which should be considered for coloring.
    /// </summary>
    /// <param name="cx">Coordinate on X axis of the grid. Zooming for the cocrete point.</param>
    /// <param name="cy">Coordinate on Y axis of the grid. Zooming for the cocrete point.</param>
    /// <param name="maxiter">Maximal number of iterations.</param>
    /// <returns>Iteration value for concrete particle of the grid.</returns>
    protected int Mandeliter(float cx, float cy, int maxiter)
    {
        int i;
        float x = 0.0f;
        float y = 0.0f;
        for (i = 0; i < maxiter && x * x + y * y <= 4; ++i)
        {
            float tmp = 2 * x * y;
            x = x * x - y * y + cx;
            y = tmp + cy;
        }
        return i;
    }

    /// <summary>
    /// The function to perfom Mandelbrot Set algorithm.
    /// </summary>
    public void Mandelbrot()
    {

        for (int ix = 0; ix < width; ++ix)
            for (int iy = 0; iy < height; ++iy)
            {
                float x = xmin + (xmax - xmin) * ix / (width - 1);
                float y = ymin + (ymax - ymin) * iy / (height - 1);
                float i = Mandeliter(x, y, maxIterations);

                // Find the color for the pixel.
                ColorThePixel(ix, iy, i);
            }

        texture.Apply();
    }

    /// <summary>
    /// Calculates the color for the picture.
    /// </summary>
    /// <param name="ix">Coordinate on X axis of the grid.</param>
    /// <param name="iy">Coordinate on Y axis of the grid.</param>
    /// <param name="i">An iteration value.</param>
    public void ColorThePixel(int ix, int iy, float i)
    {
        if (i == maxIterations)
        {
            texture.SetPixel(ix, iy, Color.black);
        }
        else
        {
            float c = 3 * Mathf.Log(i) / Mathf.Log((float)(maxIterations - 1.0));
            if (c < 1)
            {
                texture.SetPixel(ix, iy, new Color(c, 0, 0, 1));
            }
            else if (c < 2)
            {
                texture.SetPixel(ix, iy, new Color(1, c - 1, 0, 1));
            }
            else
            {
                texture.SetPixel(ix, iy, new Color(1, 1, c - 2, 1));
            }
        }
    }
}
