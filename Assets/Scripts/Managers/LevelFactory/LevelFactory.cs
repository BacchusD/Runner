using Assets.Scripts;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Подразумевается создание разных локаций. Вне зависимости от локации используются одни и те же префабы,
/// но для разных уровней будет использоватся разный mesh для визуального отображения.
/// Так еже уровнеь может иметь особенности генерации.
/// Например, есть идея для бонусного уровня, который выглядит как облака и представляет собой элементы дороги на одном уровне
/// но со множеством провалов. Игрок будет перепрыгивать с облака на облако
/// Этот класс будет отвечать за адекватную генерацию уровня, и в дальшнейшем будет опираться на настройки уровня
/// </summary>
class LevelFactory  {

        private static RoadUnit _start = BlockPositionCombine.Bottom;

        private static RoadManager _roadManager;

        private static BarrierManager _barrierManager;

        private RoadUnit _lastUnit;

        private double barrierProbability = 0.2; 

        public LevelFactory() {
            _roadManager = RoadManager.Instance;
            _barrierManager = BarrierManager.Instance;
        }

        public void AddStartRoadUnit(GameObject baseObject) {
            RoadManager.Instance.Instantiate(baseObject, _start);
            _lastUnit = _start;
        }

        public void AddNextUnit(GameObject baseObject) {
            var levelUnit = RoadUnitJoin[_lastUnit];
            _barrierManager.InstantiateWall(baseObject, (BlockPosition)levelUnit.RandomBarierType(barrierProbability));
            _roadManager.Instantiate(baseObject, levelUnit.RandomRoadType());
        }

        /// <summary>
        ///Определяет структуру генерации. Ключ - последный элемент,
        ///RoadUnit - продолжение дороги, сгруппированное по сложности,
        ///BarrierUnit - допустимые препятсвия(чтоб стена не висела в воздухе или не пересекалась с подьемами или 2 ярусом
        /// </summary>
        IDictionary<RoadUnit, LevelUnit> RoadUnitJoin = new Dictionary<RoadUnit, LevelUnit>() {
                {
                    BlockPositionCombine.Bottom,
                    new LevelUnit(
                        new RoadUnit[][] {
                            new RoadUnit[] {
                                BlockPositionCombine.Bottom
                            },
                            new RoadUnit[] {
                                new RoadUnit(RoadType.RoadSlopUp, RoadType.Road, RoadType.Road),  //,BlockPositionCombine.LeftAndRightBottom, BlockPositionCombine.RightDoubleBottom, BlockPositionCombine.LeftAndRightBottom, 
                            },
                            new RoadUnit[] {
                                BlockPosition.MiddleBottom//, BlockPosition.LeftBottom, BlockPosition.RightBottom
                            }
                       },
                    new WallType[] {
                        WallType.LeftBottom,
                        WallType.MiddleBottom,
                        WallType.RightBottom,
                        WallType.CenterWall,
                        WallType.RightWall,
                        WallType.LeftBottom,
                        WallType.CenterSlide,
                    }
                )},
            {
                new RoadUnit(RoadType.RoadSlopUp, RoadType.Road, RoadType.Road),
                new LevelUnit(
                    new RoadUnit[][] {
                        new RoadUnit[] {
                            new RoadUnit(RoadType.None, RoadType.Road, RoadType.Road, RoadType.Road, RoadType.None, RoadType.None)
                        },
                    },
                    new WallType[] {
                        WallType.MiddleBottom,
                        WallType.RightBottom,
                        WallType.CenterWall,
                        WallType.RightWall,
                    })
            },
            {
                new RoadUnit(RoadType.None, RoadType.Road, RoadType.Road, RoadType.Road, RoadType.None, RoadType.None),
                new LevelUnit(
                    new RoadUnit[][] {
                        new RoadUnit[] {
                            new RoadUnit(RoadType.None, RoadType.Road, RoadType.Road, RoadType.Road, RoadType.None, RoadType.None),
                            new RoadUnit(RoadType.RoadSlopDown, RoadType.Road, RoadType.Road)
                        },
                    },
                    new WallType[] {
                        WallType.MiddleBottom,
                        WallType.RightBottom,
                        WallType.CenterWall,
                        WallType.RightWall,
                    })
            },
            {
                new RoadUnit(RoadType.RoadSlopDown, RoadType.Road, RoadType.Road),
                new LevelUnit(
                    new RoadUnit[][] {
                        new RoadUnit[] {
                            BlockPositionCombine.Bottom
                        },
                    },
                    new WallType[] {
                        WallType.MiddleBottom,
                        WallType.RightBottom,
                        WallType.CenterWall,
                        WallType.RightWall,
                    })
            },
            {
                BlockPosition.MiddleBottom,
                new LevelUnit(
                    new RoadUnit[][] {
                        new RoadUnit[] {
                            BlockPosition.MiddleBottom,
                            BlockPositionCombine.Bottom
                        },
                    },
                    new WallType[] {
                        WallType.MiddleBottom
                    })
            }

        };


        private enum WallType {
            LeftBottom = BlockPosition.LeftBottom,
            MiddleBottom = BlockPosition.MiddleBottom,
            RightBottom = BlockPosition.RightBottom,
            LeftDoubleBottom = BlockPositionCombine.LeftDoubleBottom,
            LeftAndRightBottom = BlockPositionCombine.LeftAndRightBottom,
            RightDoubleBottom = BlockPositionCombine.RightDoubleBottom,
            LeftWall = BlockPositionCombine.LeftWall,
            CenterWall = BlockPositionCombine.CenterWall,
            RightWall = BlockPositionCombine.RightWall,
            Bottom = BlockPositionCombine.Bottom,
            LeftDouble = BlockPosition.LeftBottom | BlockPosition.LeftMiddle,
            CenterDouble = BlockPosition.MiddleBottom | BlockPosition.Center,
            RightDouble = BlockPosition.RightBottom | BlockPosition.RightMiddle,
            //left slide
            LeftSlide = BlockPositionCombine.RightDoubleBottom | BlockPositionCombine.Middle | BlockPositionCombine.Top,
            //center slide
            CenterSlide = BlockPositionCombine.LeftAndRightBottom | BlockPositionCombine.Middle | BlockPositionCombine.Top,
            //right slide
            RightSlide = BlockPositionCombine.LeftDoubleBottom | BlockPositionCombine.Middle | BlockPositionCombine.Top,
        };

        /// <summary>
        /// Определяет набор элементов дорог с группировкой и набор препятствий
        /// Предоставляет API для выбора случайного элемента
        /// </summary>
        class LevelUnit {

            /// <summary>
            /// Инициализируем ГСЧ один раз
            /// </summary>
            private static System.Random random = new System.Random();

            /// <summary>
            /// Инициализируем ГСЧ один раз. В дальнейшем можно вынести  λ - параметр в конструктор тем самым регулируя уровень сложности
            /// </summary>
            private static RandomExponential randomExp = new RandomExponential(2);

            /// <summary>
            /// сгрупированный список доступных типов дорог
            /// </summary>
            private RoadUnit[][] _roadJoint;

            private WallType[] _barrierJoint;

            private LevelUnit() { }

            public LevelUnit(RoadUnit[][] roadJoint, WallType[] barrierJoint) {
                _roadJoint = roadJoint;
                _barrierJoint = barrierJoint;
            }

            /// <summary>
            /// Возвращает случайный тип дороги из достуных
            /// вероятность выбора каждой следующей группы снижается экспоненциально
            /// вероятность выбора элемента в группе равновероятна
            /// </summary>
            /// <returns></returns>
            public RoadUnit RandomRoadType() {
                var group = randomExp.Next(_roadJoint.Length);
                var element = random.Next(_roadJoint[group].Length);
                return _roadJoint[group][element];
            }

            /// <summary>
            /// С указанной вероятностью возвращает случайное препятсвие
            /// </summary>
            /// <param name="probability">Вероятность появления препятсвия 0..1</param>
            /// <returns></returns>
            public WallType RandomBarierType(double probability) {
                return random.NextDouble() < probability ? _barrierJoint[random.Next(_barrierJoint.Length)] : 0;
            }
        }
    }

