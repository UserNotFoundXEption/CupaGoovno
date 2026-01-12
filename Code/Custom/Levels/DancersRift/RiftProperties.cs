using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CupaGoovno.RiftManager;
using static CupaGoovno.RiftManager.Enemies;
using static CupaGoovno.RiftManager.Column;
using UnityEngine;

namespace CupaGoovno;

public class RiftProperties
{
    public float timePerBeat = 60 / 153f;
    public int columnLength = 9;

    public float[] parryTimeMaxDiff = [0.05f, 0.1f, 0.2f, 0.3f];
    public int[] pointsMultiplierCombos = [10, 20, 40];
    public int[] points = [4, 3, 2, 1];
    public int[] rankPoints = [5000, 4500, 4000, 3500, 3000, 2500, 0];
    public char[] rankNames = ['S', 'A', 'B', 'C', 'D', 'E', 'F'];

    public List<EnemySpawnDelay> spawnDelays = [
        new(Enemies.ElderKettle, 10, Middle),

        new(Enemies.Slime, 14, Column.Middle, "r"),

        new(Enemies.Slime, 18, Column.Middle, "l"),

        new(Enemies.ElderKettle, 21, Column.Middle),
        new(Enemies.Slime, 22, Column.Middle, "r"),

        new(Enemies.ElderKettle, 29, Column.Middle),
        new(Enemies.Slime, 30, Column.Middle, "l"),

        new(Enemies.ElderKettle, 35, Column.Middle),

        new(Enemies.ElderKettle, 37, Column.Middle),
        new(Enemies.Slime, 38, Column.Middle, "l"),

        new(Enemies.Slime, 45, Column.Middle, "l"),
        new(Enemies.Slime, 46, Column.Right, "l"),

        new(Enemies.Slime, 49, Column.Middle, "r"),
        new(Enemies.Slime, 50, Column.Left, "r"),

        new(Enemies.Slime, 53, Column.Middle, "l"),
        new(Enemies.Slime, 54, Column.Right, "l"),

        new(Enemies.Cupcake, 63, Column.Left),

        new(Enemies.Cupcake, 67, Column.Right),

        new(Enemies.ElderKettle, 70, Column.Middle),
        new(Enemies.ElderKettle, 71, Column.Left),

        new(Enemies.Slime, 74, Column.Right, "l"),
        new(Enemies.ElderKettle, 75, Column.Left),
        new(Enemies.Slime, 76, Column.Left, "r"),
        new(Enemies.ElderKettle, 77, Column.Right),

        new(Enemies.ElderKettle, 78, Column.Middle),
        new(Enemies.ElderKettle, 79, Column.Middle),
        new(Enemies.ElderKettle, 80, Column.Middle),
        new(Enemies.ElderKettle, 81, Column.Middle),
        new(Enemies.ElderKettle, 82, Column.Middle),
        new(Enemies.ElderKettle, 83, Column.Middle),
        new(Enemies.ElderKettle, 84, Column.Middle),
        new(Enemies.ElderKettle, 85, Column.Middle),
        new(Enemies.Bird, 86, Column.Middle, "l2"),

        new(Enemies.Slime, 90, Column.Middle, "l"),

        new(Enemies.ElderKettle, 94, Column.Middle),
        new(Enemies.ElderKettle, 95, Column.Middle),
        new(Enemies.ElderKettle, 96, Column.Middle),
        new(Enemies.ElderKettle, 97, Column.Middle),
        new(Enemies.ElderKettle, 98, Column.Right),
        new(Enemies.ElderKettle, 99, Column.Right),
        new(Enemies.ElderKettle, 100, Column.Right),
        new(Enemies.ElderKettle, 101, Column.Right),
        new(Enemies.Slime, 102, Column.Right, "l"),

        new(Enemies.Slime, 105, Column.Middle, "l"),
        new(Enemies.Slime, 109, Column.Middle, "r"),

        new(Enemies.Bird, 112, Column.Left, "r2"),
        new(Enemies.Bird, 116, Column.Right, "l2"),
        new(Enemies.Bird, 120, Column.Left, "r2"),
        new(Enemies.Bird, 124, Column.Right, "l2"),

        new(Enemies.Cupcake, 129, Column.Left),
        new(Enemies.Cupcake, 133, Column.Right),
        new(Enemies.Bird, 136, Column.Left, "r3"),

        new(Enemies.Slime, 144, Column.Right, "l"),
        new(Enemies.Slime, 145, Column.Middle, "r"),
        new(Enemies.Bird, 146, Column.Right, "l3"),

        new(Enemies.Bird, 151, Column.Middle, "l2"),
        new(Enemies.Bird, 153, Column.Middle, "r2"),

        new(Enemies.ElderKettle, 156, Column.Middle),
        new(Enemies.ElderKettle, 157, Column.Right),
        new(Enemies.ElderKettle, 158, Column.Middle),
        new(Enemies.ElderKettle, 159, Column.Right),
        new(Enemies.ElderKettle, 160, Column.Middle),
        new(Enemies.ElderKettle, 161, Column.Right),
        new(Enemies.ElderKettle, 162, Column.Middle),

        new(Enemies.Slime, 164, Column.Left, "r"),
        new(Enemies.Slime, 165, Column.Middle, "l"),
        new(Enemies.Slime, 166, Column.Left, "r"),
        new(Enemies.Slime, 167, Column.Middle, "l"),
        new(Enemies.Slime, 168, Column.Left, "r"),
        new(Enemies.Slime, 169, Column.Middle, "l"),
        new(Enemies.Slime, 170, Column.Left, "r"),
        new(Enemies.Slime, 171, Column.Middle, "l"),

        new(Enemies.Cupcake, 174, Column.Middle),
        new(Enemies.ElderKettle, 182, Column.Middle),

        new(Enemies.Cupcake, 190, Column.Middle),
        new(Enemies.Cupcake, 192, Column.Middle),
        new(Enemies.Cupcake, 194, Column.Middle),
        new(Enemies.Cupcake, 196, Column.Middle),
        new(Enemies.Cupcake, 198, Column.Middle),
        new(Enemies.Cupcake, 200, Column.Middle),
        new(Enemies.Cupcake, 202, Column.Middle),
        new(Enemies.Cupcake, 204, Column.Middle),
        new(Enemies.Cupcake, 206, Column.Left),
        new(Enemies.Cupcake, 208, Column.Right),
        new(Enemies.Cupcake, 210, Column.Left),
        new(Enemies.Cupcake, 212, Column.Right),

        new(Enemies.ElderKettle, 214, Column.Middle),
        new(Enemies.Slime, 215, Column.Middle, "l"),
        new(Enemies.ElderKettle, 216, Column.Middle),
        new(Enemies.Slime, 217, Column.Middle, "r"),
        new(Enemies.ElderKettle, 218, Column.Middle),
        new(Enemies.Slime, 219, Column.Middle, "l"),
        new(Enemies.ElderKettle, 220, Column.Middle),

        new(Enemies.Bird, 222, Column.Left, "r3"),
        new(Enemies.ElderKettle, 225, Column.Middle),
        new(Enemies.Bird, 226, Column.Right, "l3"),

        new(Enemies.Bird, 230, Column.Right, "l3"),
        new(Enemies.ElderKettle, 233, Column.Middle),
        new(Enemies.Bird, 234, Column.Left, "r3"),

        new(Enemies.Bird, 238, Column.Left, "r3"),
        new(Enemies.ElderKettle, 241, Column.Middle),
        new(Enemies.Bird, 242, Column.Right, "l3"),

        new(Enemies.Bird, 246, Column.Right, "l3"),
        new(Enemies.ElderKettle, 249, Column.Middle),
        new(Enemies.Bird, 250, Column.Left, "r3"),

        new(Enemies.ElderKettle, 254, Column.Middle),
        new(Enemies.ElderKettle, 258, Column.Middle),
        new(Enemies.ElderKettle, 262, Column.Middle),
        new(Enemies.ElderKettle, 266, Column.Middle),

        new(Enemies.Slime, 270, Column.Left, "r"),
        new(Enemies.Slime, 271, Column.Right, "l"),
        new(Enemies.Slime, 272, Column.Left, "r"),

        new(Enemies.Slime, 274, Column.Right, "l"),
        new(Enemies.Slime, 275, Column.Left, "r"),
        new(Enemies.Slime, 276, Column.Right, "l"),

        new(Enemies.Slime, 278, Column.Middle, "l"),
        new(Enemies.Slime, 279, Column.Middle, "l"),
        new(Enemies.Slime, 280, Column.Right, "l"),
        new(Enemies.Slime, 281, Column.Right, "l"),
        new(Enemies.Slime, 282, Column.Middle, "r"),
        new(Enemies.Slime, 283, Column.Middle, "r"),
        new(Enemies.Slime, 284, Column.Left, "r"),
        new(Enemies.Slime, 285, Column.Left, "r"),
        new(Enemies.Cupcake, 286, Column.Middle),

        new(Enemies.Slime, 287, Column.Middle, "l"),
        new(Enemies.Slime, 288, Column.Right, "l"),
        new(Enemies.Slime, 289, Column.Middle, "r"),

        new(Enemies.Cupcake, 290, Column.Middle),
        new(Enemies.Slime, 291, Column.Middle, "l"),
        new(Enemies.Cupcake, 292, Column.Middle),
        new(Enemies.Slime, 293, Column.Middle, "r"),

        new(Enemies.Bird, 297, Column.Middle, "l1"),
        new(Enemies.Bird, 300, Column.Left, "r2"),

        new(Enemies.ElderKettle, 306, Column.Left),
        new(Enemies.ElderKettle, 307, Column.Middle),
        new(Enemies.Bird, 308, Column.Middle, "l2"),

        new(Enemies.Bird, 313, Column.Middle, "l1"),
        new(Enemies.Bird, 317, Column.Middle, "l1"),

        new(Enemies.Bird, 318, Column.Left, "r3"),
        new(Enemies.Bird, 322, Column.Right, "l3"),
        new(Enemies.Bird, 326, Column.Left, "r3"),

        new(Enemies.Bird, 330, Column.Middle, "l2"),
        new(Enemies.Bird, 332, Column.Middle, "r2"),
        new(Enemies.Cupcake, 334, Column.Middle),
        new(Enemies.Cupcake, 335, Column.Middle),
        new(Enemies.Cupcake, 336, Column.Middle),
        new(Enemies.Cupcake, 337, Column.Middle),
        new(Enemies.Cupcake, 338, Column.Middle),
        new(Enemies.Cupcake, 339, Column.Middle),

        new(Enemies.Bird, 342, Column.Left, "r3"),
        new(Enemies.Bird, 346, Column.Right, "l3"),

        new(Enemies.Slime, 350, Column.Middle, "r"),
        new(Enemies.Slime, 351, Column.Left, "r"),
        new(Enemies.Slime, 352, Column.Middle, "r"),
        new(Enemies.Slime, 353, Column.Left, "r"),
        new(Enemies.Slime, 354, Column.Middle, "l"),
        new(Enemies.Slime, 355, Column.Right, "l"),
        new(Enemies.Slime, 356, Column.Middle, "l"),
        new(Enemies.Slime, 357, Column.Right, "l"),

        new(Enemies.Bird, 358, Column.Left, "r3"),
        new(Enemies.Bird, 362, Column.Right, "l3"),

        new(Enemies.ElderKettle, 367, Column.Middle),
        new(Enemies.ElderKettle, 371, Column.Middle),
        new(Enemies.ElderKettle, 375, Column.Middle),
        new(Enemies.ElderKettle, 379, Column.Middle),

        new(Enemies.ElderKettle, 389, Column.Middle),
        new(Enemies.Slime, 390, Column.Middle, "l"),

        new(Enemies.ElderKettle, 397, Column.Middle),
        new(Enemies.Slime, 398, Column.Middle, "r"),

        new(Enemies.ElderKettle, 405, Column.Middle),
        new(Enemies.Slime, 406, Column.Middle, "l"),

        new(Enemies.ElderKettle, 413, Column.Middle),
        new(Enemies.Slime, 414, Column.Middle, "r"),

        new(Enemies.ElderKettle, 421, Column.Middle),
        new(Enemies.Slime, 422, Column.Middle, "l"),

        new(Enemies.ElderKettle, 429, Column.Middle),
        new(Enemies.Slime, 430, Column.Middle, "r"),

        new(Enemies.Bird, 435, Column.Middle, "r2"),

        new(Enemies.Bird, 441, Column.Left, "r3"),
        new(Enemies.Bird, 444, Column.Middle, "r2"),

        new(Enemies.Bird, 448, Column.Left, "r2"),
        new(Enemies.Bird, 450, Column.Right, "l2"),
        new(Enemies.Cupcake, 452, Column.Middle),
        new(Enemies.Cupcake, 453, Column.Middle),
        new(Enemies.Bird, 454, Column.Left, "r2"),
        new(Enemies.Bird, 456, Column.Right, "l2"),
        new(Enemies.Bird, 458, Column.Left, "r2"),
        new(Enemies.Bird, 460, Column.Right, "l2"),

        new(Enemies.Cupcake, 467, Column.Middle),

        new(Enemies.ElderKettle, 470, Column.Left),
        new(Enemies.Slime, 471, Column.Left, "r"),
        new(Enemies.ElderKettle, 472, Column.Right),
        new(Enemies.Slime, 473, Column.Right, "l"),
        new(Enemies.ElderKettle, 474, Column.Left),
        new(Enemies.Slime, 475, Column.Left, "r"),
        new(Enemies.ElderKettle, 476, Column.Right),
        new(Enemies.Slime, 477, Column.Right, "l"),
        new(Enemies.Cupcake, 478, Column.Middle),

        new(Enemies.ElderKettle, 480, Column.Left),
        new(Enemies.Bird, 483, Column.Right, "l3"),
        new(Enemies.ElderKettle, 487, Column.Right),

        new(Enemies.ElderKettle, 491, Column.Middle),
        new(Enemies.Cupcake, 492, Column.Left),
        new(Enemies.ElderKettle, 493, Column.Middle),
        new(Enemies.Cupcake, 494, Column.Right),
        new(Enemies.ElderKettle, 496, Column.Middle),

        new(Enemies.Cupcake, 498, Column.Middle),
        new(Enemies.Cupcake, 499, Column.Middle),
        new(Enemies.Cupcake, 500, Column.Middle),
        new(Enemies.Cupcake, 501, Column.Middle),
        new(Enemies.Cupcake, 502, Column.Middle),
        new(Enemies.Cupcake, 503, Column.Middle),
        new(Enemies.Cupcake, 504, Column.Middle),
        new(Enemies.Cupcake, 505, Column.Middle),
        new(Enemies.Cupcake, 506, Column.Middle),
        new(Enemies.Cupcake, 507, Column.Middle),
        new(Enemies.ElderKettle, 508, Column.Right),

        new(Enemies.ElderKettle, 511, Column.Middle),
        new(Enemies.ElderKettle, 512, Column.Right),

        new(Enemies.ElderKettle, 515, Column.Middle),
        new(Enemies.Bird, 516, Column.Left, "r3"),
        new(Enemies.ElderKettle, 519, Column.Right),

        new(Enemies.ElderKettle, 521, Column.Right),
        new(Enemies.ElderKettle, 522, Column.Right),

        new(Enemies.Bird, 526, Column.Left, "r2"),
        new(Enemies.Bird, 530, Column.Right, "l2"),
        new(Enemies.Bird, 534, Column.Left, "r2"),
        new(Enemies.Bird, 538, Column.Right, "l2"),

        new(Enemies.ElderKettle, 542, Column.Middle),
        new(Enemies.ElderKettle, 546, Column.Middle),
        new(Enemies.Cupcake, 550, Column.Middle),
        new(Enemies.Cupcake, 552, Column.Middle),
        new(Enemies.Cupcake, 554, Column.Middle),
        new(Enemies.Cupcake, 556, Column.Middle),
        new(Enemies.Cupcake, 558, Column.Middle),
        new(Enemies.Cupcake, 560, Column.Middle),
        new(Enemies.Cupcake, 562, Column.Middle),
        new(Enemies.Cupcake, 564, Column.Middle),

        new(Enemies.Bird, 566, Column.Left, "r2"),
        new(Enemies.Slime, 568, Column.Middle, "r"),
        new(Enemies.Bird, 569, Column.Right, "l1"),
        new(Enemies.Cupcake, 570, Column.Middle, "l1"),
        new(Enemies.Bird, 571, Column.Middle, "r2"),

        new(Enemies.Bird, 574, Column.Right, "l2"),
        new(Enemies.Slime, 576, Column.Middle, "l"),
        new(Enemies.Bird, 577, Column.Left, "r1"),
        new(Enemies.Cupcake, 578, Column.Middle, "r1"),
        new(Enemies.Bird, 579, Column.Middle, "l2"),

        new(Enemies.Bird, 582, Column.Left, "r2"),
        new(Enemies.Slime, 584, Column.Middle, "r"),
        new(Enemies.Bird, 585, Column.Right, "l1"),
        new(Enemies.Cupcake, 586, Column.Middle, "l1"),
        new(Enemies.Bird, 587, Column.Middle, "r2"),

        new(Enemies.Bird, 590, Column.Right, "l2"),
        new(Enemies.Slime, 592, Column.Middle, "l"),
        new(Enemies.Bird, 593, Column.Left, "r1"),
        new(Enemies.Cupcake, 594, Column.Middle, "r1"),
        new(Enemies.Bird, 595, Column.Middle, "l2"),

        new(Enemies.Bird, 598, Column.Left, "r2"),
        new(Enemies.Slime, 600, Column.Middle, "r"),
        new(Enemies.Bird, 601, Column.Right, "l1"),
        new(Enemies.Bird, 602, Column.Right, "l2"),
        new(Enemies.Slime, 604, Column.Middle, "l"),
        new(Enemies.Bird, 605, Column.Left, "l1"),

        new(Enemies.ElderKettle, 606, Column.Middle),
        new(Enemies.ElderKettle, 607, Column.Right),

        new(Enemies.ElderKettle, 610, Column.Middle),
        new(Enemies.ElderKettle, 611, Column.Right),
        new(Enemies.Cupcake, 612, Column.Right),

        new(Enemies.ElderKettle, 614, Column.Middle),
        new(Enemies.ElderKettle, 615, Column.Right),

        new(Enemies.ElderKettle, 618, Column.Middle),
        new(Enemies.ElderKettle, 619, Column.Right),
        new(Enemies.Cupcake, 620, Column.Right),

        new(Enemies.ElderKettle, 622, Column.Middle),
        new(Enemies.ElderKettle, 623, Column.Right),

        new(Enemies.ElderKettle, 626, Column.Middle),
        new(Enemies.ElderKettle, 627, Column.Right),
        new(Enemies.Cupcake, 628, Column.Right),

        new(Enemies.Slime, 630, Column.Left, "r"),
        new(Enemies.Slime, 631, Column.Middle, "l"),
        new(Enemies.Slime, 632, Column.Right, "l"),
        new(Enemies.Slime, 633, Column.Middle, "l"),
        new(Enemies.Slime, 634, Column.Left, "r"),
        new(Enemies.Slime, 635, Column.Middle, "l"),
        new(Enemies.Slime, 636, Column.Right, "l"),
        new(Enemies.Slime, 637, Column.Middle, "l"),
        new(Enemies.Slime, 638, Column.Left, "r"),
        new(Enemies.Slime, 639, Column.Middle, "r"),
        new(Enemies.Slime, 640, Column.Right, "l"),
        new(Enemies.Slime, 641, Column.Middle, "r"),
        new(Enemies.Slime, 642, Column.Left, "r"),
        new(Enemies.Slime, 643, Column.Middle, "r"),
        new(Enemies.Slime, 644, Column.Right, "l"),
        new(Enemies.Slime, 645, Column.Middle, "r"),
        new(Enemies.Cupcake, 646, Column.Middle),

        new(Enemies.ElderKettle, 655, Column.Middle),
        ];

    public Dictionary<Column, List<Vector2>> enemyPositions = new()
    {
        { Left, new List<Vector2> {
            new(-190f, -240f),
            new(-158f, -90f),
            new(-133f, 11f),
            new(-115f, 85f),
            new(-101f, 141f),
            new(-91f, 186f),
            new(-81f, 222f),
            new(-74f, 252f),
            new(-68f, 285f),
            new(-62f, 320f) } },
        { Middle, new List<Vector2> {
            new(0f, -240f),
            new(0f, -90f),
            new(0f, 11f),
            new(0f, 85f),
            new(0f, 141f),
            new(0f, 186f),
            new(0f, 222f),
            new(0f, 252f),
            new(0f, 285f),
            new(0f, 320f) } },
        { Right, new List<Vector2> {
            new(190f, -240f),
            new(158f, -90f),
            new(133f, 11f),
            new(115f, 85f),
            new(101f, 141f),
            new(91f, 186f),
            new(81f, 222f),
            new(74f, 252f),
            new(68f, 285f),
            new(62f, 320f) } }
    };

    public List<float> enemyScale = [1f, 0.8f, 0.7f, 0.6f, 0.53f, 0.47f, 0.43f, 0.38f, 0.33f, 0.28f];
    public float enemyMoveTime = 0.1f;

    public class String
    {
        public float[] x = [-202f, 0f, 202f];
        public float[] y = [-273f, -276f, -273f];
        public float[] rotation = [-13f, 0f, 13f];
    }

    public class Popup
    {
        public Dictionary<Column, Vector2> positions = new()
        {
            { Column.Left, new(0.35f, 0.2f) },
            { Column.Middle, new(0.5f, 0.2f) },
            { Column.Right, new(0.65f, 0.2f) }
        };

        public Dictionary<ParryScore, Color> colors = new()
        {
            { ParryScore.Miss, Color.red },
            { ParryScore.Ok, Color.white },
            { ParryScore.Good, Color.green },
            { ParryScore.Great, Color.cyan },
            { ParryScore.Perfect, new(0.6f, 0f, 1f) },
        };

        public float durationIntro = 0.5f;
        public float durationFade = 1f;
        public float speed = 50f;
    }
}
